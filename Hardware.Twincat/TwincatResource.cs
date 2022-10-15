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
        /// <param name="code">The code</param>
        /// <param name="amsNetAddress">The PLC ams net address</param>
        /// <param name="port">The port number</param>
        /// <param name="pollingInterval">The polling interval (in milliseconds)</param>
        /// <param name="maximumDelayBetweenNotifications">The maximum delay between each ads notification (in millisecond)</param>
        public TwincatResource(string code, string amsNetAddress, int port, int pollingInterval = 100, int maximumDelayBetweenNotifications = 20)
            : this(code, port, pollingInterval, maximumDelayBetweenNotifications)
        {
            initializedWithAddress = true;
            address = new AmsAddress(amsNetAddress, port);

            InitializeResource();
        }

        /// <summary>
        /// Initialize a new instance of <see cref="TwincatResource"/> by specifying only the port number of the Ads server
        /// </summary>
        /// <remarks>This version of the constructor should be used when the connection has to be established to a local ADS server (only the port is needed in this case)</remarks>
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
    }
}