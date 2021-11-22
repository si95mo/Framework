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
    public class TwincatResource : Resource, IResource
    {
        private bool isOpen;

        private string amsNetAddress;
        private int port;

        private AdsClient client;
        private AmsAddress address;

        private Dictionary<string, uint> variableHandles;

        public override bool IsOpen => isOpen;

        /// <summary>
        /// The ams net address
        /// </summary>
        public string AmsNetAddress => amsNetAddress;

        /// <summary>
        /// The port number
        /// </summary>
        public int Port => port;

        public TwincatResource(string code, string amsNetAddress, int port) : base(code)
        {
            this.amsNetAddress = amsNetAddress;
            this.port = port;

            address = new AmsAddress(amsNetAddress, port);
            client = new AdsClient();

            client.AdsNotificationError += (object _, AdsNotificationErrorEventArgs e) =>
            {
                Logger.Error($"Ads error. Recevied: {e.Exception.Message}");
                Status.Value = ResourceStatus.Failure;
            };

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
            await client.ConnectAndWaitAsync(address, CancellationToken.None);

            if(client.Session.IsConnected)
            {
                Status.Value = ResourceStatus.Executing;
                isOpen = true;
            }    
            else
            {
                HandleException($"{code}- Unable to connect to {amsNetAddress}:{port}");
            }
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
            {
                HandleException($"{code}- Unable to disconnect to {amsNetAddress}:{port}");
            }
        }

        /// <summary>
        /// Receive a value through the <see cref="TwincatResource"/>
        /// </summary>
        /// <param name="code">The channel code in which the value will be stored</param>
        /// <returns>The async <see cref="Task"/></returns>
        internal async Task Receive(string code)
        {
            if(Status.Value == ResourceStatus.Executing)
            {
                ITwincatChannel channel = channels.Get(code) as ITwincatChannel;
                uint handle = variableHandles[channel.Code];

                Memory<byte> buffer = new Memory<byte>();
                await client.ReadAsync(handle, buffer, CancellationToken.None);

                if (channel is TwincatAnalogInput)
                    (channel as TwincatAnalogInput).Value = Convert.ToDouble(buffer);
                else // Digital input
                    (channel as TwincatDigitalInput).Value = Convert.ToBoolean(buffer);
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