using Unosquare.RaspberryIO.Abstractions;

namespace Hardware.Raspberry
{
    /// <summary>
    /// Implement an output GPIO <see cref="PiChannel{T}"/>
    /// </summary>
    public class PiDigitalOutput : PiChannel<bool>
    {
        private IResource resource;

        /// <summary>
        /// Create a new instance of <see cref="PiDigitalOutput"/>
        /// </summary>
        public PiDigitalOutput() : base()
        { }

        /// <summary>
        /// Create a new instance of <see cref="PiDigitalOutput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="pinNumber">The pin number</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        public PiDigitalOutput(string code, int pinNumber, IResource resource) : base(code, pinNumber, resource)
        {
            PinMode = GpioPinDriveMode.Output;
            this.resource = resource;

            ValueChanged += PiOutputChannel_ValueChanged;
        }

        private void PiOutputChannel_ValueChanged(object sender, Core.ValueChangedEventArgs e)
            => (resource as PiGpioResource).Send(PinNumber, Value);
    }
}