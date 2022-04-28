using System;
using Unosquare.RaspberryIO.Abstractions;

namespace Hardware.Raspberry
{
    public abstract class PiChannel : Channel<bool>, IPiChannel
    {
        public GpioPinDriveMode PinMode
        { get; protected set; }

        private int pinNumber;

        public int PinNumber { get => pinNumber; set => pinNumber = value; }

        /// <summary>
        /// Create a new instance of <see cref="PiInputChannel"/>
        /// </summary>
        protected PiChannel() : base(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Create a new instance of <see cref="PiInputChannel"/>
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