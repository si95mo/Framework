using Unosquare.RaspberryIO.Abstractions;

namespace Hardware.Raspberry
{
    /// <summary>
    /// Implement an output GPIO <see cref="PiChannel{T}"/>
    /// </summary>
    public class PiOutputChannel : PiChannel<bool>
    {
        /// <summary>
        /// Create a new instance of <see cref="PiOutputChannel"/>
        /// </summary>
        public PiOutputChannel() : base()
        { }

        /// <summary>
        /// Create a new instance of <see cref="PiOutputChannel"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="pinNumber">The pin number</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        public PiOutputChannel(string code, int pinNumber, IResource resource) : base(code, pinNumber, resource)
        {
            PinMode = GpioPinDriveMode.Output;
        }
    }
}