using Core;
using Core.DataStructures;
using Diagnostic;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TwinCAT.Ads;

namespace Hardware.Twincat
{
    /// <summary>
    /// Implement a resource that communicate with TwinCAT via the Ads protocol
    /// </summary>
    public class TwincatResource : Resource, IResource
    {
        private bool isOpen;

        private string amsNetAddress;
        private int port;

        private AdsClient client;
        private AmsAddress address;

        private Dictionary<string, uint> variableHandles;

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
            client = new AdsClient();

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

            client = new AdsClient();
            InitializeResource();
        }

        /// <summary>
        /// Initialize the <see cref="TwincatResource"/> status
        /// </summary>
        private void InitializeResource()
        {
            client.AdsNotificationError += (object _, AdsNotificationErrorEventArgs e) =>
            {
                Logger.Error($"Ads error. Recevied: {e.Exception.Message}");
                Status.Value = ResourceStatus.Failure;
            };
            client.Timeout = 5000;

            variableHandles = new Dictionary<string, uint>();
            channels.ItemAdded += Channels_ItemAdded;
            channels.ItemRemoved += Channels_ItemRemoved;

            Status.Value = ResourceStatus.Stopped;
        }

        private async void Channels_ItemAdded(object sender, BagChangedEventArgs<IProperty> e)
        {
            ITwincatChannel channel = e.Item as ITwincatChannel;
            ResultHandle resultHandler = await client.CreateVariableHandleAsync(channel.VariableName, CancellationToken.None);
            uint handle = resultHandler.Handle;

            if (!variableHandles.ContainsKey(channel.Code))
                variableHandles.Add(channel.Code, handle);
            else
                variableHandles[channel.Code] = handle;
        }

        private async void Channels_ItemRemoved(object sender, BagChangedEventArgs<IProperty> e)
        {
            ITwincatChannel channel = e.Item as ITwincatChannel;
            if (variableHandles.TryGetValue(channel.Code, out uint handle))
                await client.DeleteVariableHandleAsync(handle, CancellationToken.None);

            variableHandles.Remove(channel.Code);
        }

        public override async Task Restart()
        {
            Stop();
            await Start();
        }

        public override async Task Start()
        {
            Status.Value = ResourceStatus.Starting;

            Task timeoutTask = new Task(async () => await Task.Delay(client.Timeout));
            Task t;
            if (initializedWithAddress)
                t = await Task.WhenAny(client.ConnectAndWaitAsync(address, CancellationToken.None), timeoutTask);
            else
                t = await Task.WhenAny(Task.Run(() => client.Connect(port)), timeoutTask);

            if (t == timeoutTask)
                HandleException($"{code} - Unable to connect to {amsNetAddress}:{port}");
            else
            {
                if (client.Session != null)
                {
                    if (client.Session.IsConnected)
                    {
                        Status.Value = ResourceStatus.Executing;
                        isOpen = true;
                    }
                    else
                        HandleException($"{code} - Unable to connect to {amsNetAddress}:{port}");
                }
                else
                {
                    if (client.IsConnected)
                    {
                        Status.Value = ResourceStatus.Executing;
                        isOpen = true;
                    }
                    else
                        HandleException($"{code} - Unable to connect to {amsNetAddress}:{port}");
                }
            }

            if (Status.Value == ResourceStatus.Executing)
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Receive();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        public override void Stop()
        {
            Status.Value = ResourceStatus.Stopping;
            client.Close();

            if (!client.Session.IsConnected)
            {
                Status.Value = ResourceStatus.Stopped;
                isOpen = false;
            }
            else
                HandleException($"{code}- Unable to disconnect to {amsNetAddress}:{port}");
        }

        /// <summary>
        /// Receive a value through the <see cref="TwincatResource"/>
        /// </summary>
        /// <returns>The async <see cref="Task"/></returns>
        private async Task Receive()
        {
            while (Status.Value == ResourceStatus.Executing)
            {
                uint handle;
                Memory<byte> buffer;

                foreach (IChannel channel in channels)
                {
                    handle = variableHandles[channel.Code];

                    buffer = new Memory<byte>();
                    await client.ReadAsync(handle, buffer, CancellationToken.None);

                    if (channel is TwincatAnalogInput)
                        (channel as TwincatAnalogInput).Value = Convert.ToDouble(buffer);
                    else // Digital input
                        (channel as TwincatDigitalInput).Value = Convert.ToBoolean(buffer);
                }

                await Task.Delay(PollingInterval);
            }
        }

        /// <summary>
        /// Send a value through the <see cref="TwincatResource"/>
        /// </summary>
        /// <param name="code">The channel code with the value to send</param>
        /// <returns>The async <see cref="Task"/></returns>
        internal async Task Send(string code)
        {
            if (Status.Value == ResourceStatus.Executing)
            {
                ITwincatChannel channel = channels.Get(code) as ITwincatChannel;
                uint handle = variableHandles[channel.Code];

                Memory<byte> buffer;

                if (channel is TwincatAnalogOutput)
                    buffer = BitConverter.GetBytes((channel as TwincatAnalogOutput).Value);
                else
                    buffer = BitConverter.GetBytes((channel as DigitalOutput).Value);

                await client.WriteAsync(handle, buffer, CancellationToken.None);
            }
        }
    }
}