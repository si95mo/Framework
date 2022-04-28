using Unosquare.RaspberryIO.Abstractions;

namespace Hardware.Raspberry
{
    /// <summary>
    /// Implement an input GPIO <see cref="Channel{T}"/>
    /// </summary>
    internal class PiInputChannel : PiChannel
    {
        /// <summary>
        /// Create a new instance of <see cref="PiInputChannel"/>
        /// </summary>
        public PiInputChannel() : base()
        { }

        /// <summary>
        /// Create a new instance of <see cref="PiInputChannel"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="pinNumber">The pin number</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        public PiInputChannel(string code, int pinNumber, IResource resource) : base(code, pinNumber, resource)
        {
            PinMode = GpioPinDriveMode.Input;
        }
    }
}