using Unosquare.RaspberryIO.Abstractions;

namespace Hardware.Raspberry
{
    /// <summary>
    /// Implement an input GPIO <see cref="PiChannel{T}"/>
    /// </summary>
    internal class PiDigitalInput : PiChannel<bool>
    {
        /// <summary>
        /// Create a new instance of <see cref="PiDigitalInput"/>
        /// </summary>
        public PiDigitalInput() : base()
        { }

        /// <summary>
        /// Create a new instance of <see cref="PiDigitalInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="pinNumber">The pin number</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        public PiDigitalInput(string code, int pinNumber, IResource resource) : base(code, pinNumber, resource)
        {
            PinMode = GpioPinDriveMode.Input;
        }
    }
}