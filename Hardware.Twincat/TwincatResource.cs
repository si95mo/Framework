using Core;
using Core.DataStructures;
using Diagnostic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// Maximum notifications allowed
        /// </summary>
        private const int MaxNotificationsCounter = 512;
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

        private Dictionary<string, int> variableHandles;
        private HashSet<string> arrayNames;

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
        /// Create a new instance of <see cref="TwincatResource"/>
        /// by specifying both the ams net address and the port of the Ads server
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="amsNetAddress">The PLC ams net address</param>
        /// <param name="port">The port number</param>
        /// <param name="pollingInterval">The polling interval (in milliseconds)</param>
        public TwincatResource(string code, string amsNetAddress, int port, int pollingInterval = 100, int maximumDelayBetweenNotifications = 20) : base(code)
        {
            this.amsNetAddress = amsNetAddress;
            this.port = port;
            PollingInterval = pollingInterval;
            MaximumDelayBetweenNotifications = maximumDelayBetweenNotifications;

            initializedWithAddress = true;

            address = new AmsAddress(amsNetAddress, port);
            client = new TcAdsClient();

            InitializeResource();
        }

        /// <summary>
        /// Initialize a new instance of <see cref="TwincatResource"/>
        /// by specifying only the port number of the Ads server
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="port">The port number</param>
        /// <param name="pollingInterval">The polling interval (in milliseconds)</param>
        public TwincatResource(string code, int port, int pollingInterval = 100) : base(code)
        {
            this.port = port;
            amsNetAddress = "";
            PollingInterval = pollingInterval;

            initializedWithAddress = false;

            client = new TcAdsClient();

            InitializeResource();
        }

        /// <summary>
        /// Initialize the <see cref="TwincatResource"/> status
        /// </summary>
        private void InitializeResource()
        {
            symbolLoader = (AdsSymbolLoader)SymbolLoaderFactory.Create(client, SymbolLoaderSettings.Default);
            symbolLoader.DefaultNotificationSettings = new NotificationSettings(NotificationTransactionMode, PollingInterval, MaximumDelayBetweenNotifications);            

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

            variableHandles = new Dictionary<string, int>();
            arrayNames = new HashSet<string>();

            Channels.ItemAdded += Channels_ItemAdded;
            Channels.ItemRemoved += Channels_ItemRemoved;

            Status.Value = ResourceStatus.Stopped;
        }

        private void ConnectToVariables(ITwincatChannel channel)
        {
            if (client.IsConnected)
            {
                int handle = client.CreateVariableHandle("");

                if (!variableHandles.ContainsKey(channel.Code))
                    variableHandles.Add(channel.Code, handle);
                else
                    variableHandles[channel.Code] = handle;
            }
            else
            {
                if (client.Disposed)
                    Logger.Error($"Unable to connect {channel.Code} to {channel.VariableName}");
            }
        }

        private void DisconnectFromVariables(ITwincatChannel channel)
        {
            if (client.IsConnected)
            {
                if (variableHandles.TryGetValue(channel.Code, out int handle))
                    client.DeleteVariableHandle(handle);

                variableHandles.Remove(channel.Code);
            }
            else
            {
                if (client.Disposed)
                    Logger.Error($"Unable to disconnect {channel.Code} from {channel.VariableName}");
            }
        }

        private void Channels_ItemAdded(object sender, BagChangedEventArgs<IProperty> e)
        {
            ITwincatChannel channel = e.Item as ITwincatChannel;

            if (channel.ArrayName.CompareTo(string.Empty) != 0)
                arrayNames.Add(channel.ArrayName);

            ConnectToVariables(channel);
        }

        private void Channels_ItemRemoved(object sender, BagChangedEventArgs<IProperty> e)
        {
            ITwincatChannel channel = e.Item as ITwincatChannel;

            if (channel.ArrayName.CompareTo(string.Empty) != 0)
                arrayNames.Remove(channel.ArrayName);

            DisconnectFromVariables(channel);
        }

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
                    client.Connect(address);

                if (client.ConnectionState == ConnectionState.Connected)
                {
                    Status.Value = ResourceStatus.Executing;
                    isOpen = true;
                }
                else
                    HandleException($"{Code} - Unable to connect to {amsNetAddress}:{port}");

                if (Status.Value == ResourceStatus.Executing)
                {
                    if (Channels.Count > 0)
                        Channels.ToList().ForEach((x) => ConnectToVariables(x as ITwincatChannel));

#pragma warning disable CS4014
                    Receive();
#pragma warning restore CS4014
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

        /// <summary>
        /// Receive a value through the <see cref="TwincatResource"/>
        /// </summary>
        /// <returns>The async <see cref="Task"/></returns>
        private async Task Receive()
        {
            int handle, timeToWait;
            ITwincatChannel twincatChannel;
            Stopwatch timer;

            while (Status.Value == ResourceStatus.Executing)
            {
                timer = Stopwatch.StartNew();
                try
                {
                    foreach (string arrayName in arrayNames)
                    {
                        ITcAdsSymbol symbol = client.ReadSymbolInfo(arrayName);
                        IEnumerable<object> enumerable = client.ReadSymbol(symbol) as IEnumerable<object>;

    
                        //if (twincatChannel is TwincatAnalogInput) // Analog input
                        //    (twincatChannel as TwincatAnalogInput).Value = Convert.ToDouble(client.ReadSymbol(symbol));
                        //else // Analog output, digital output or digital input
                        //{
                        //    if (twincatChannel is TwincatDigitalInput) // Digital input
                        //        (twincatChannel as TwincatDigitalInput).Value = Convert.ToBoolean(client.ReadSymbol(symbol));
                        //}
                    }
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }

                timer.Stop();
                if (PollingInterval < timer.ElapsedMilliseconds)
                    await Logger.WarnAsync($"Polling cycle exceeded the set time by {Math.Abs(PollingInterval - timer.ElapsedMilliseconds):0.0}[ms]");

                await Logger.InfoAsync($"Polling cycle took {timer.ElapsedMilliseconds:0.0}[ms]");

                timeToWait = PollingInterval < timer.ElapsedMilliseconds ? PollingInterval : (int)(PollingInterval - timer.ElapsedMilliseconds);
                await Task.Delay(timeToWait);
            }
        }

        /// <summary>
        /// Send a value through the <see cref="TwincatResource"/>
        /// </summary>
        /// <param name="code">The channel code with the value to send</param>
        internal void Send(string code)
        {
            if (Status.Value == ResourceStatus.Executing)
            {
                ITwincatChannel channel = Channels.Get(code) as ITwincatChannel;

                try
                {
                    client.WriteSymbol(channel.VariableName, channel.ValueAsObject, true);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Tries to get the specified instance
        /// </summary>
        /// <param name="channel">The <see cref="ITwincatChannel"/></param>
        /// <param name="symbol">The <see cref="ISymbol"/></param>
        /// <returns><see langword=""="true"/> if the operation succeeded, <see langword="false"/> otherwise</returns>
        internal bool TryGetInstance(ITwincatChannel channel, out ISymbol symbol)
            => symbolLoader.Symbols.TryGetInstance(channel.VariableName, out symbol);

        private void AttachChannel(ITwincatChannel channel)
        {
            string errorMessage = string.Empty;
            try
            {
                if (symbolLoader.Symbols.TryGetInstance(channel.VariableName, out ISymbol symbol))
                {
                    Type symbolManagedType = (symbol.DataType as DataType).ManagedType;
                    if(symbolManagedType.IsNumeric())
                }
            }
        }
    }
}