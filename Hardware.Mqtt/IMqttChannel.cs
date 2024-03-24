using Core.Parameters;

namespace Hardware.Mqtt
{
    public enum SubscriptionStatus
    {
        /// <summary>
        /// The subscription was successful
        /// </summary>
        Success,

        /// <summary>
        /// The subscription failed
        /// </summary>
        Failure
    }

    /// <summary>
    /// Define a basic MQTT channel
    /// </summary>
    /// <typeparam name="T">The wrapped value type</typeparam>
    public interface IMqttChannel<T> : IChannel
    {
        /// <summary>
        /// The topic to subscribe to
        /// </summary>
        string Topic { get; }

        /// <summary>
        /// Define the status of the subscription
        /// </summary>
        EnumParameter<SubscriptionStatus> Status { get; }
    }
}

