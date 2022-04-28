using Unosquare.RaspberryIO.Abstractions;

namespace Hardware.Raspberry
{
    public class PiOutputChannel : PiChannel
    {
        /// <summary>
        /// Create a new instance of <see cref="PiInputChannel"/>
        /// </summary>
        public PiOutputChannel() : base()
        { }

        /// <summary>
        /// Create a new instance of <see cref="PiInputChannel"/>
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