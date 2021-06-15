using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardware.Serial
{
    class SerialResource : SerialPort, IResource
    {
        private string code;
        private ResourceStatus status;

        /// <summary>
        /// Create a new instance of <see cref="SerialResource"/> and opens it in order to
        /// perform serial communication.
        /// See <see cref="SerialPort"/> and <see cref="IResource"/>.
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="portName">The port name</param>
        /// <param name="baudRate">The baud rate</param>
        /// <param name="parity">The parity type</param>
        /// <param name="dataBits">Data bits number</param>
        /// <param name="stopbits">Stop bit type</param>
        public SerialResource(string code, string portName, int baudRate = 9600, Parity parity = Parity.None, 
            int dataBits = 8, StopBits stopbits = StopBits.One) : base(portName, baudRate, parity, dataBits, stopbits)
        {
            this.code = code;

            Open();
        }

        /// <summary>
        /// The <see cref="SerialResource"/> code
        /// </summary>
        public string Code => code;

        /// <summary>
        /// The <see cref="SerialResource"/> status
        /// </summary>
        public ResourceStatus Status => status;

        /// <summary>
        /// Start the <see cref="SerialResource"/>
        /// </summary>
        public void Start()
        {
            Open();
        }

        /// <summary>
        /// Stop the <see cref="SerialResource"/>
        /// </summary>
        public void Stop()
        {
            Stop();
        }

        public void Restart()
        {

        }
    }
}
