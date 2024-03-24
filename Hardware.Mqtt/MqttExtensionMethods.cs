using MQTTnet.Protocol;

namespace Hardware.Mqtt
{
    /// <summary>
    /// Define MQTT-related extension methods
    /// </summary>
    public static class MqttExtensionMethods
    {
        /// <summary>
        /// Convert a <see cref="QualityOfService"/> to a <see cref="MqttQualityOfServiceLevel"/>
        /// </summary>
        /// <param name="source">The <see cref="QualityOfService"/> value to convert</param>
        /// <returns>The conversion result</returns>
        internal static MqttQualityOfServiceLevel ToLibraryQualityOfService(this QualityOfService source)
        {
            MqttQualityOfServiceLevel result = source == QualityOfService.AtMostOnce ? 
                MqttQualityOfServiceLevel.AtMostOnce : source == QualityOfService.AtLeastOnce ? MqttQualityOfServiceLevel.AtLeastOnce : MqttQualityOfServiceLevel.ExactlyOnce;

            return result;
        }
    }
}
