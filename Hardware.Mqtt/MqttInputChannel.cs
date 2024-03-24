using Diagnostic;
using System.Threading;
using System.Threading.Tasks;

namespace Hardware.Mqtt
{
    /// <summary>
    /// Define an input channel for MQTT that will subscribe to an MQTT topic
    /// </summary>
    public class MqttInputChannel<T> : MqttChannel<T>
    {
        private readonly ManualResetEvent manualResetEvent;

        /// <summary>
        /// Create a new instance of <see cref="MqttInputChannel{T}{T}"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="resource">The <see cref="MqttClientResource"/></param>
        /// <param name="topic">The topic to subscribe to</param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        public MqttInputChannel(string code, MqttClientResource resource, string topic, string measureUnit = "", string format = "") : base(code, resource, topic, measureUnit, format)
        {
            manualResetEvent = new ManualResetEvent(false);

            bool subscribed = false;
            Task.Run(async () =>
                {
                    subscribed = await resource.SubscribeAsync(this, topic);
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
    }
}
