using Core;
using Diagnostic;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TwinCAT;
using TwinCAT.Ads;
using TwinCAT.Ads.TypeSystem;
using TwinCAT.TypeSystem;

namespace Hardware.Twincat
{
    /// <summary>
    /// Implement a resource that communicate with TwinCAT via the Ads protocol
    /// </summary>
    public class TwincatResource : Resource
    {
        /// <summary>
        /// The <see cref="AdsTransMode"/>
        /// </summary>
        private const AdsTransMode NotificationTransactionMode = AdsTransMode.OnChange;

        private bool isOpen;

        private string amsNetAddress;
        private int port;

        private AmsAddress address;
        private TcAdsClient client;
        private AdsSymbolLoader symbolLoader;

        private bool initializedWithAddress;

        public override bool IsOpen => isOpen;

        /// <summary>
        /// The ams net address
        /// </summary>
        public string AmsNetAddress => amsNetAddress;

        /// <summary>
        /// The port number
        /// </summary>
        public int Port => port;

        /// <summary>
        /// The <see cref="TwincatResource"/> polling interval (in milliseconds)
        /// </summary>
        public int PollingInterval { get; set; }

        /// <summary>
        /// The maximum delay between each notification (in milliseconds)
        /// </summary>
        public int MaximumDelayBetweenNotifications { get; set; }

        /// <summary>
        /// Create a new instance of <see cref="TwincatResource"/> by specifying both the ams net address and the port of the Ads server
        /// </summary>
        /// <remarks>
        /// If <paramref name="amsNetAddress"/> is equal to <see cref="string.Empty"/>, then the connection will be established to the local ads server
        /// </remarks>
        /// <param name="code">The code</param>
        /// <param name="amsNetAddress">The PLC ams net address</param>
        /// <param name="port">The port number</param>
        /// <param name="pollingInterval">The polling interval (in milliseconds)</param>
        /// <param name="maximumDelayBetweenNotifications">The maximum delay between each ads notification (in millisecond)</param>
        public TwincatResource(string code, string amsNetAddress, int port, int pollingInterval = 100, int maximumDelayBetweenNotifications = 20)
            : this(code, port, pollingInterval, maximumDelayBetweenNotifications)
        {
            initializedWithAddress = amsNetAddress != string.Empty;
            address = new AmsAddress(amsNetAddress, port);

            InitializeResource();
        }

        /// <summary>
        /// Initialize a new instance of <see cref="TwincatResource"/> by specifying only the port number of the Ads server
        /// </summary>
        /// <remarks>
        /// This version of the constructor should be used when the connection has to be established to a local ADS server (only the port is needed in this case)
        /// </remarks>
        /// <param name="code">The code</param>
        /// <param name="port">The port number</param>
        /// <param name="pollingInterval">The polling interval (in milliseconds)</param>
        /// <param name="maximumDelayBetweenNotifications">The maximum delay between each ads notification (in millisecond)</param>
        public TwincatResource(string code, int port, int pollingInterval = 100, int maximumDelayBetweenNotifications = 20) : base(code)
        {
            this.port = port;
            amsNetAddress = string.Empty;
            PollingInterval = pollingInterval;
            MaximumDelayBetweenNotifications = maximumDelayBetweenNotifications;

            initializedWithAddress = false;
            client = new TcAdsClient();

            InitializeResource();
        }

        #region Resource implementation

        public override async Task Restart()
        {
            Stop();
            await Start();
        }

        public override Task Start()
        {
            Status.Value = ResourceStatus.Starting;

            try
            {
                if (initializedWithAddress)
                    client.Connect(address.NetId, address.Port);
                else
                    client.Connect(port);

                if (client.ConnectionState == ConnectionState.Connected)
                {
                    symbolLoader = (AdsSymbolLoader)SymbolLoaderFactory.Create(client, SymbolLoaderSettings.Default);
                    symbolLoader.DefaultNotificationSettings = new NotificationSettings(NotificationTransactionMode, PollingInterval, MaximumDelayBetweenNotifications);

                    Status.Value = ResourceStatus.Executing;
                    isOpen = true;
                }
                else
                    HandleException($"{Code} - Unable to connect to {amsNetAddress}:{port}");

                if (Status.Value == ResourceStatus.Executing)
                {
                    if (Channels.Count > 0)
                        Channels.ToList().ForEach((x) => (x as ITwincatChannel).Attach());
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return Task.CompletedTask;
        }

        public override void Stop()
        {
            Status.Value = ResourceStatus.Stopping;

            client.Disconnect();
            client.Close();

            if (client.Session == null)
            {
                Status.Value = ResourceStatus.Stopped;
                isOpen = false;
            }
            else
                HandleException($"{Code} - Unable to disconnect to {amsNetAddress}:{port}");
        }

        #endregion Resource implementation

        #region RPC implementation

        /// <summary>
        /// Call an RPC (Remote Procedure Call) method
        /// </summary>
        /// <typeparam name="T">The return value type</typeparam>
        /// <param name="symbolPath">The method instance path (i.e. MAIN.fbCalculator)</param>
        /// <param name="methodName">The method name (i.e. Sum)</param>
        /// <param name="parameters">The array of parameters</param>
        /// <returns>The result of the call</returns>
        /// <exception cref="Exception">An <see cref="Exception"/> in case of invalid RPC call</exception>
        public T InvokeRpcMethod<T>(string symbolPath, string methodName, object[] parameters)
        {
            if (Status.Value != ResourceStatus.Executing)
                Logger.Error($"Unable to invoke {symbolPath}.{methodName} from {Code}. Resource not executing");

            lock (client)
            {
                // Check if the input is valid
                foreach (object parameter in parameters)
                    if (!IsAValidType(parameter.GetType()))
                    {
                        Logger.Error($"Invalid input type for {symbolPath}.{methodName}");
                        throw new Exception("Invalid input type");
                    }

                // Check if the output is valid
                if (!IsAValidType(typeof(T)))
                {
                    Logger.Error($"Invalid output type for {symbolPath}.{methodName}");
                    throw new Exception("Invalid output type");
                }

                // Create the handle
                int handle = client.CreateVariableHandle($"{symbolPath}#{methodName}");

                int returnSize = 0;
                if (typeof(T).IsArray) // If the output is an array
                {
                    IRpcStructInstance rpcInstance;
                    if (symbolLoader.Symbols.TryGetInstance(symbolPath, out ISymbol symbol))
                        rpcInstance = (IRpcStructInstance)symbol;
                    else
                    {
                        Logger.Error($"Provided symbol ({symbolPath}) does not exist");
                        throw new Exception("Provided symbol does not exist");
                    }

                    IRpcMethod rpcMethod = rpcInstance.RpcMethods.FirstOrDefault(m => m.Name == methodName);
                    if (rpcMethod == null)
                    {
                        Logger.Error($"RPC method name ({methodName}) does not exist");
                        throw new Exception("RPC method name does not exist");
                    }

                    returnSize = rpcMethod.ReturnTypeSize;
                }
                else // If the output is a value type (int, double, short, ...)
                {
                    returnSize = Marshal.SizeOf<T>();
                }

                AdsStream reader = new AdsStream(returnSize);
                int sizeOfInputs = parameters.Select(p => GetParameterSize(p)).Sum();
                AdsStream writer = new AdsStream(sizeOfInputs);

                AdsBinaryReader binaryReader = new AdsBinaryReader(reader);
                AdsBinaryWriter binaryWriter = new AdsBinaryWriter(writer);

                reader.Position = 0;
                writer.Position = 0;

                // Set the RPC call arguments
                SetArguments(binaryWriter, parameters);

                try
                {
                    // And then all PLC method
                    int byteCount = client.ReadWrite((uint)AdsReservedIndexGroups.SymbolValueByHandle, (uint)handle, reader, writer);

                    T result = default;
                    if (byteCount > 0)
                        result = GetReturnValue<T>(binaryReader, returnSize);

                    client.DeleteVariableHandle(handle);

                    return result;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    throw new Exception("Error invoking RPC method. See inner exception", ex);
                }
            }
        }

        #region Helper methods

        /// <summary>
        /// Determine wheather <paramref name="type"/> is valid or not
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to test</param>
        /// <returns><see langword="true"/> if <paramref name="type"/> is valid, <see langword="false"/> otherwise</returns>
        private bool IsAValidType(Type type)
        {
            bool result = type.IsValueType || (type.IsArray && type.GetElementType().IsValueType && type.GetArrayRank() == 1);
            return result;
        }

        /// <summary>
        /// Set the arguments of an RPC call
        /// </summary>
        /// <param name="binaryWriter">The <see cref="AdsBinaryWriter"/></param>
        /// <param name="parameters">The parameters array</param>
        /// <exception cref="Exception">An <see cref="Exception"/> in case of invalid parameters <see cref="Type"/></exception>
        private void SetArguments(AdsBinaryWriter binaryWriter, params object[] parameters)
        {
            foreach (object parameter in parameters)
            {
                Type parameterType = parameter.GetType();

                if (!parameterType.IsArray)
                {
                    if (parameter is double valueAsDouble)
                        binaryWriter.Write(valueAsDouble);
                    else if (parameter is float valueAsFloat)
                        binaryWriter.Write(valueAsFloat);
                    else if (parameter is string valueAsString)
                        binaryWriter.Write(valueAsString);
                    else if (parameter is int valueAsInt)
                        binaryWriter.Write(valueAsInt);
                    else if (parameter is short valueAsShort)
                        binaryWriter.Write(valueAsShort);
                    else if (parameter is bool valueAsBool)
                        binaryWriter.Write(valueAsBool);
                    else
                        throw new Exception("Invalid parameter type");
                }
                else
                {
                    for (int i = 0; i < (parameter as Array).Length; i++)
                    {
                        if (parameterType == typeof(double[]))
                            binaryWriter.Write((parameter as double[])[i]);
                        else if (parameterType == typeof(float[]))
                            binaryWriter.Write((parameter as float[])[i]);
                        else if (parameterType == typeof(string[]))
                            binaryWriter.Write((parameter as string[])[i]);
                        else if (parameterType == typeof(int[]))
                            binaryWriter.Write((parameter as int[])[i]);
                        else if (parameterType == typeof(short[]))
                            binaryWriter.Write((parameter as short[])[i]);
                        else if (parameterType == typeof(bool[]))
                            binaryWriter.Write((parameter as bool[])[i]);
                        else
                            throw new Exception("Invalid parameter array type");
                    }
                }
            }
        }

        /// <summary>
        /// Get the return value of an RPC call
        /// </summary>
        /// <typeparam name="T">The return <see cref="Type"/></typeparam>
        /// <param name="binaryReader">The <see cref="AdsBinaryReader"/></param>
        /// <param name="byteCount">The number of bytes</param>
        /// <returns>The return value</returns>
        /// <exception cref="Exception">An <see cref="Exception"/> in case of invalid parameters <see cref="Type"/></exception>
        private T GetReturnValue<T>(AdsBinaryReader binaryReader, int byteCount)
        {
            if (typeof(T) == typeof(double)) return (T)(object)binaryReader.ReadDouble();
            else if (typeof(T) == typeof(float)) return (T)(object)binaryReader.ReadSingle();
            else if (typeof(T) == typeof(string)) return (T)(object)binaryReader.ReadString();
            else if (typeof(T) == typeof(int)) return (T)(object)binaryReader.ReadInt32();
            else if (typeof(T) == typeof(short)) return (T)(object)binaryReader.ReadInt16();
            else if (typeof(T) == typeof(bool)) return (T)(object)binaryReader.ReadBoolean();
            else if (typeof(T) == typeof(double[])) return (T)(object)GetRPCReturnArray<double>(binaryReader, byteCount);
            else if (typeof(T) == typeof(float[])) return (T)(object)GetRPCReturnArray<float>(binaryReader, byteCount);
            else if (typeof(T) == typeof(string[])) return (T)(object)GetRPCReturnArray<string>(binaryReader, byteCount);
            else if (typeof(T) == typeof(int[])) return (T)(object)GetRPCReturnArray<int>(binaryReader, byteCount);
            else if (typeof(T) == typeof(short[])) return (T)(object)GetRPCReturnArray<short>(binaryReader, byteCount);
            else if (typeof(T) == typeof(bool[])) return (T)(object)GetRPCReturnArray<bool>(binaryReader, byteCount);
            else throw new Exception("Invalid return data type");
        }

        /// <summary>
        /// Get the return value of an RPC call in case of an array
        /// </summary>
        /// <typeparam name="T">The return <see cref="Type"/></typeparam>
        /// <param name="binaryReader">The <see cref="AdsBinaryReader"/></param>
        /// <param name="byteCount">The number of bytes</param>
        /// <returns>The return value</returns>
        private T[] GetRPCReturnArray<T>(AdsBinaryReader binaryReader, int byteCount)
        {
            byte[] resultAsByteArray = binaryReader.ReadBytes(byteCount);
            T[] result = new T[resultAsByteArray.Length / Marshal.SizeOf<T>()];

            Buffer.BlockCopy(resultAsByteArray, 0, result, 0, byteCount);

            return result;
        }

        /// <summary>
        /// Get a <paramref name="parameter"/> size in bytes
        /// </summary>
        /// <param name="parameter">The parameter of which retrieve the size</param>
        /// <returns>The size of <paramref name="parameter"/></returns>
        private int GetParameterSize(object parameter)
        {
            int size = parameter.GetType().IsArray ?
                GetTypeSizeInBeckhoff(parameter.GetType().GetElementType()) * (parameter as Array).GetLength(0) :
                GetTypeSizeInBeckhoff(parameter.GetType());

            return size;
        }

        /// <summary>
        /// Get the size of a type as defined by Beckhoff
        /// </summary>
        /// <param name="dotNetType">The <see cref="Type"/> inside the .net framework</param>
        /// <returns>The retrieved size</returns>
        private int GetTypeSizeInBeckhoff(Type dotNetType)
        {
            int size = dotNetType == typeof(bool) ? 1 : Marshal.SizeOf(dotNetType);
            return size;
        }

        #endregion Helper methods

        #endregion RPC implementation

        #region Helper methods

        /// <summary>
        /// Initialize the <see cref="TwincatResource"/> status
        /// </summary>
        private void InitializeResource()
        {
            client.AdsNotificationError += (object _, AdsNotificationErrorEventArgs e) =>
            {
                string failureDescription = e.Exception.Message;

                LastFailure = new Failure(failureDescription);
                Status.Value = ResourceStatus.Failure;

                Logger.Error($"Ads error. Received: {failureDescription}");
            };
            client.AdsStateChanged += (object _, AdsStateChangedEventArgs e) =>
                Logger.Warn($"Ads state change notification. Actual Ads state: {e.State.AdsState}");

            client.ConnectionStateChanged += (object _, ConnectionStateChangedEventArgs e) =>
            {
                Logger.Warn($"Connection state change notification. Actual state: {e.NewState}");

                if (e.NewState == ConnectionState.Connected)
                    Status.Value = ResourceStatus.Executing;
                else
                {
                    if (e.NewState == ConnectionState.Disconnected)
                        Status.Value = ResourceStatus.Stopped;
                    else
                        Status.Value = ResourceStatus.Failure;
                }
            };

            client.Timeout = 5000;

            Status.Value = ResourceStatus.Stopped;
        }

        /// <summary>
        /// Tries to get the specified instance
        /// </summary>
        /// <param name="channel">The <see cref="ITwincatChannel"/></param>
        /// <param name="symbol">The <see cref="ISymbol"/></param>
        /// <returns><see langword=""="true"/> if the operation succeeded, <see langword="false"/> otherwise</returns>
        internal bool TryGetInstance(ITwincatChannel channel, out ISymbol symbol)
            => symbolLoader.Symbols.TryGetInstance(channel.VariableName, out symbol);

        #endregion Helper methods
    }
}