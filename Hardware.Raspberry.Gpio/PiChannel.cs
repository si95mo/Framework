using System;
using Unosquare.RaspberryIO.Abstractions;

namespace Hardware.Raspberry
{
    /// <summary>
    /// Define a basic implementation of a channel to be used with <see cref="PiGpioResource"/>
    /// </summary>
    public abstract class PiChannel<T> : Channel<T>, IPiChannel
    {
        public GpioPinDriveMode PinMode
        { get; protected set; }

        private int pinNumber;

        public int PinNumber { get => pinNumber; set => pinNumber = value; }

        /// <summary>
        /// Create a new instance of <see cref="PiDigitalInput"/>
        /// </summary>
        protected PiChannel() : base(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Create a new instance of <see cref="PiDigitalInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="pinNumber">The pin number</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        public PiChannel(string code, int pinNumber, IResource resource) : base(code, "", "", resource)
        {
            this.pinNumber = pinNumber;
        }
    }
}