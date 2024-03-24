using Core.Parameters;
using Newtonsoft.Json;

namespace Hardware.Mqtt
{
    public abstract class MqttChannel<T> : Channel<T>, IMqttChannel<T>
    {
        public string Topic { get; private set; }
        public EnumParameter<SubscriptionStatus> Status { get; private set; }

        /// <summary>
        /// Create a new instance of <see cref="MqttChannel{T}"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="resource">The <see cref="MqttClientResource"/></param>
        /// <param name="topic">The topic to subscribe to</param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        protected MqttChannel(string code, MqttClientResource resource, string topic, string measureUnit = "", string format = "") : base(code, measureUnit, format, resource)
        {
            Topic = topic;
            Status = new EnumParameter<SubscriptionStatus>($"{Code}.Status", SubscriptionStatus.Failure);
        }

        /// <summary>
        /// Set the actual value of the channel from the payload received from the <see cref="MqttClientResource"/>"/>
        /// </summary>
        /// <param name="value">The value to set</param>
        internal virtual void SetValue(string value)
        {
            Value = JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// Get the serialized value of the channel <see cref="Channel{T}.Value"/>
        /// </summary>
        /// <returns>The serialized <see cref="Channel{T}.Value"/></returns>
        internal string GetSerializedValue()
        {
            return JsonConvert.SerializeObject(Value);
        }
    }
}
