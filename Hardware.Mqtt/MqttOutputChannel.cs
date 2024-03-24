namespace Hardware.Mqtt
{
    /// <summary>
    /// Create an output channel for MQTT that will publish to an MQTT topic
    /// </summary>
    /// <typeparam name="T">The wrapped value type</typeparam>
    public class MqttOutputChannel<T> : MqttChannel<T>
    {
        public QualityOfService QualityOfService { get; private set; }

        /// <summary>
        /// Create a new instance of <see cref="MqttOutputChannel{T}"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="resource">The <see cref="MqttClientResource"/></param>
        /// <param name="topic">The topic</param>
        /// <param name="qualityOfService">The <see cref="Mqtt.QualityOfService"/></param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        public MqttOutputChannel(string code, MqttClientResource resource, string topic, QualityOfService qualityOfService, string measureUnit = "", string format = "") : base(code, resource, topic, measureUnit, format)
        {
            QualityOfService = qualityOfService;
            ValueChanged += async delegate 
            { 
                await resource.PublishAsync(this, QualityOfService, Topic); 
            };
        }
    }
}
