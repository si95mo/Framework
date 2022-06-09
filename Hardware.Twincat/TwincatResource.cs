using Core;
using Core.DataStructures;
using Diagnostic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwinCAT;
using TwinCAT.Ads;

namespace Hardware.Twincat
{
    /// <summary>
    /// Implement a resource that communicate with TwinCAT via the Ads protocol
    /// </summary>
    public class TwincatResource : Resource
    {
        private bool isOpen;

        private string amsNetAddress;
        private int port;

        private TcAdsClient client;
        private AmsAddress address;

        private Dictionary<string, int> variableHandles;

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
        /// Create a new instance of <see cref="TwincatResource"/>
        /// by specifying both the ams net address and the port of the Ads server
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="amsNetAddress">The PLC ams net address</param>
        /// <param name="port">The port number</param>
        /// <param name="pollingInterval">The polling interval (in milliseconds)</param>
        public TwincatResource(string code, string amsNetAddress, int port, int pollingInterval = 100) : base(code)
        {
            this.amsNetAddress = amsNetAddress;
            this.port = port;
            PollingInterval = pollingInterval;

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
            Channels.ItemAdded += Channels_ItemAdded;
            Channels.ItemRemoved += Channels_ItemRemoved;

            Status.Value = ResourceStatus.Stopped;
        }

        private async Task ConnectToVariables(ITwincatChannel channel)
        {
            if (client.IsConnected)
            {
                int handle = client.CreateVariableHandle(channel.VariableName);

                if (!variableHandles.ContainsKey(channel.Code))
                    variableHandles.Add(channel.Code, handle);
                else
                    variableHandles[channel.Code] = handle;
            }
            else
            {
                if (client.Disposed)
                    await Logger.ErrorAsync($"Unable to connect {channel.Code} to {channel.VariableName}");
            }
        }

        private async Task DisconnectFromVariables(ITwincatChannel channel)
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
                    await Logger.ErrorAsync($"Unable to disconnect {channel.Code} from {channel.VariableName}");
            }
        }

        private void Channels_ItemAdded(object sender, BagChangedEventArgs<IProperty> e)
        {
            ITwincatChannel channel = e.Item as ITwincatChannel;
            Task.Run(async () => await ConnectToVariables(channel));
        }

        private void Channels_ItemRemoved(object sender, BagChangedEventArgs<IProperty> e)
        {
            ITwincatChannel channel = e.Item as ITwincatChannel;
            Task.Run(async () => await DisconnectFromVariables(channel));
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
                        Channels.ToList().ForEach(async (x) => await ConnectToVariables(x as ITwincatChannel));

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
            int handle;
            ITwincatChannel twincatChannel;

            while (Status.Value == ResourceStatus.Executing)
            {
                try
                {
                    foreach (IChannel channel in Channels)
                    {
                        twincatChannel = (ITwincatChannel)channel;

                        if (variableHandles.TryGetValue(twincatChannel.Code, out handle))
                        {
                            ITcAdsSymbol symbol = client.ReadSymbolInfo(twincatChannel.VariableName);

                            if (twincatChannel is TwincatAnalogInput) // Analog input
                                (twincatChannel as TwincatAnalogInput).Value = Convert.ToDouble(client.ReadSymbol(symbol));
                            else // Analog output, digital output or digital input
                            {
                                if (twincatChannel is TwincatDigitalInput) // Digital input
                                    (twincatChannel as TwincatDigitalInput).Value = Convert.ToBoolean(client.ReadSymbol(symbol));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }

                await Task.Delay(PollingInterval);
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
    }
}