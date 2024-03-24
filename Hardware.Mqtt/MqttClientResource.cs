using Diagnostic;
using Extensions;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hardware.Mqtt
{
    /// <summary>
    /// Define an MQTT client <see cref="Resource"/>
    /// </summary>
    public class MqttClientResource : Resource
    {
        public override bool IsOpen => (bool)(client?.IsConnected);

        private readonly IMqttClient client;
        private readonly MqttClientOptions options;
        private readonly int timeoutInMilliseconds;
        private readonly object sync = new object();

        /// <summary>
        /// Create a new instance of <see cref="MqttClientResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="brokerAddress">The MQTT broker address</param>
        /// <param name="port">The port</param>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <param name="withTls">Define if the connection is secured or not</param>
        /// <param name="timeoutInMilliseconds">The timeout in milliseconds</param>
        public MqttClientResource(string code, string brokerAddress, int port = 1883, string username = null, string password = null, bool withTls = false, int timeoutInMilliseconds = 10000) : base(code)
        {
            MqttFactory factory = new MqttFactory();
            client = factory.CreateMqttClient();

            MqttClientOptionsBuilder optionsBuilder = new MqttClientOptionsBuilder()
                .WithTcpServer(brokerAddress, port)
                .WithClientId(code)
                .WithTlsOptions(new MqttClientTlsOptions { UseTls = withTls });

            if(!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                optionsBuilder = optionsBuilder.WithCredentials(username, password);
            }
            else if(!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(password))
            {
                Logger.Error("Both username and password must be provided");
                throw new ArgumentException("Both username and password must be provided");
            }

            options = optionsBuilder.Build();

            this.timeoutInMilliseconds = timeoutInMilliseconds;

            client.ConnectedAsync += Client_ConnectedAsync;
            client.DisconnectedAsync += Client_DisconnectedAsync;
        }

        /// <summary>
        /// Define if an <see cref="MqttClientSubscribeResultItem"/> is actually subscribed or not
        /// </summary>
        /// <param name="item">The <see cref="MqttClientSubscribeResultItem"/> to check</param>
        /// <returns><see langword="true"/> if <paramref name="item"/> subscribed, <see langword="false"/> otherwise</returns>
        private bool Subscribed(MqttClientSubscribeResultItem item)
        {
            bool subscribed = item != null;
            if(subscribed)
            {
                subscribed = item.ResultCode.IsIn(MqttClientSubscribeResultCode.GrantedQoS0, MqttClientSubscribeResultCode.GrantedQoS1, MqttClientSubscribeResultCode.GrantedQoS2);
            }

            return subscribed;
        }

        /// <summary>
        /// Attach the <see cref="MqttClientResource"/> to a topic
        /// </summary>
        /// <param name="topic">The topic to subscribe to</param>
        /// <returns>The (async) <see cref="Task"/>, of which the result define the subscription status</returns>
        internal async Task<bool> AttachAsync<T>(string topic, IMqttChannel<T> channel)
        {
            MqttTopicFilter options = new MqttTopicFilterBuilder().WithTopic(topic).Build();
            MqttClientSubscribeResult result = await client.SubscribeAsync(options);

            bool subscribed = false;
            if (IsOpen)
            {
                MqttClientSubscribeResultItem item = result.Items.Where((x) => x.TopicFilter.Topic == topic).LastOrDefault(); // Get the last item in case of multiple subscriptions
                subscribed = Subscribed(item);

                if (subscribed)
                {
                    await Logger.InfoAsync($"{Code} subscribed to topic \"{topic}\"");
                }
                else
                {
                    await Logger.ErrorAsync($"{Code} unable to subscribe to topic \"{topic}\"");
                }
            }
            else
            {
                await Logger.InfoAsync($"Channel with code \"{channel.Code}\" attached to {Code}, but resource not yet started or in failure");
            }

            client.ApplicationMessageReceivedAsync += async (arg) =>
            {
                await ProcessReceivedMessageAsync(channel, arg);
            };

            return subscribed;
        }

        private async Task ProcessReceivedMessageAsync<T>(IMqttChannel<T> channel, MqttApplicationMessageReceivedEventArgs arg)
        {
            await Logger.TraceAsync($"{Code} received message on topic \"{arg.ApplicationMessage.Topic}\" for channel \"{channel.Code}\"");

            lock (sync)
            {
                string payloadAsString = arg.ApplicationMessage.ConvertPayloadToString();
                channel.SetValue(payloadAsString);
            }
        }

        #region Resource implementation

        public override async Task Restart()
        {
            Stop();
            await Start();
        }

        public override async Task Start()
        {
            Status.Value = ResourceStatus.Starting;

            bool connectionCompleted = await client.ConnectAsync(options, CancellationToken.None).StartWithTimeout(timeoutInMilliseconds);

            if (connectionCompleted)
            {
                if (client.IsConnected)
                {
                    Status.Value = ResourceStatus.Executing;
                    await Logger.InfoAsync($"{Code} connected");
                }
                else
                {
                    Status.Value = ResourceStatus.Failure;
                    HandleException($"{Code} unable to connect to the specified broker");
                }
            }
            else
            {
                Status.Value = ResourceStatus.Failure;
                HandleException($"{Code} unable to connect to the specified broker, timeout elapsed");
            }
        }

        public override void Stop()
        {
            Status.Value = ResourceStatus.Stopping;
            Task.Run(async () => await client.DisconnectAsync());

            if (!client.IsConnected)
            {
                Status.Value = ResourceStatus.Stopped;
                Logger.Info($"{Code} disconnected");
            }
            else
            {
                Status.Value = ResourceStatus.Failure;
                HandleException($"{Code} unable to disconnect to the specified broker");
            }
        }

        #endregion Resource implementation

        #region Event handlers

        private async Task Client_ConnectedAsync(MqttClientConnectedEventArgs arg)
        {
            if (arg.ConnectResult.ResultCode == MqttClientConnectResultCode.Success)
            {
                await Logger.InfoAsync($"{Code} connected to broker");
            }
            else
            {
                await Logger.ErrorAsync($"{Code} unable to connect to the specified broker. Connection result code is \"{arg.ConnectResult.ResultCode}\"");
            }
        }

        private async Task Client_DisconnectedAsync(MqttClientDisconnectedEventArgs arg)
        {
            await Logger.WarnAsync($"{Code} disconnected from the specified broker");
        }

        #endregion Event handlers
    }
}
