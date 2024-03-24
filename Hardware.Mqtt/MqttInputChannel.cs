using Core.Parameters;
using Diagnostic;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Hardware.Mqtt
{
    /// <summary>
    /// Define an input channel for MQTT
    /// </summary>
    public class MqttInputChannel<T> : Channel<T>, IMqttChannel<T>
    {
        public string Topic { get; private set; }
        public EnumParameter<SubscriptionStatus> Status { get; private set; }

        private readonly ManualResetEvent manualResetEvent;

        /// <summary>
        /// Create a new instance of <see cref="MqttInputChannel{T}"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="resource">The <see cref="MqttClientResource"/></param>
        /// <param name="topic">The topic to subscribe to</param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        public MqttInputChannel(string code, MqttClientResource resource, string topic, string measureUnit = "", string format = "") : base(code, measureUnit, format, resource)
        {
            manualResetEvent = new ManualResetEvent(false);

            Topic = topic;
            Status = new EnumParameter<SubscriptionStatus>($"{Code}.Status", SubscriptionStatus.Failure);

            bool subscribed = false;
            Task.Run(async () =>
                {
                    subscribed = await resource.AttachAsync(topic, this);
                    manualResetEvent.Set();
                }
            );

            manualResetEvent.WaitOne();
            manualResetEvent.Reset();

            if (subscribed)
            {
                Status.Value = SubscriptionStatus.Success;
                Logger.Info($"{Code} subscribed to topic \"{topic}\"");
            }
            else
            {
                Status.Value = SubscriptionStatus.Failure;
                Logger.Error($"{Code} failed to subscribe to topic \"{topic}\"");
            }
        }

        public void SetValue(string value)
        {
            Value = JsonConvert.DeserializeObject<T>(value);
        }
    }
}
