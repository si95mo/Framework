using Core;

namespace Hardware
{
    /// <summary>
    /// Define a generic analog input channel
    /// </summary>
    public interface IAnalogInput : IAnalogChannel, IReadOnlyProperty<double>
    {
    }
}