using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosquare.RaspberryIO.Abstractions;

namespace Hardware.Raspberry
{
    public class PiOneWireInput : PiChannel<double>
    {
        /// <summary>
        /// Create a new instance of <see cref="PiDigitalInput"/>
        /// </summary>
        public PiOneWireInput() : base()
        { }

        /// <summary>
        /// Create a new instance of <see cref="PiDigitalInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="pinNumber">The pin number</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        public PiOneWireInput(string code, int pinNumber, IResource resource) : base(code, pinNumber, resource)
        {
            PinMode = GpioPinDriveMode.Input;
        }

        /// <summary>
        /// Read from the 1-wire bus
        /// </summary>
        internal void Read()
        {

        }
    }
}
