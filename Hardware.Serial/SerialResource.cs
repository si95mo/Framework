using Core;
using Diagnostic;
using System;
using System.IO.Ports;

namespace Hardware.Serial
{
    class SerialResource : SerialPort, IResource
    {
        private string code;
        private ResourceStatus status;
        private IFailure failure;

        /// <summary>
        /// The <see cref="SerialResource"/> code
        /// </summary>
        public string Code => code;

        /// <summary>
        /// The <see cref="SerialResource"/> status
        /// </summary>
        public ResourceStatus Status => status;

        /// <summary>
        /// The <see cref="SerialResource"/> last <see cref="IFailure"/>
        /// </summary>
        public IFailure LastFailure => failure;

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
            status = ResourceStatus.STOPPED;

            ErrorReceived += SerialResource_ErrorReceived;

            Start();
        }

        /// <summary>
        /// Handles error in the serial communication
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="SerialErrorReceivedEventArgs"/></param>
        private void SerialResource_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            status = ResourceStatus.FAILURE;

            SerialError error = e.EventType;
           
            failure.Description = error.ToString();
            failure.Timestamp = DateTime.Now;

            Logger.Log(failure.Description, SeverityType.DEBUG);
        }

        /// <summary>
        /// Start the <see cref="SerialResource"/>
        /// </summary>
        public void Start()
        {
            failure.Clear();

            status = ResourceStatus.STARTING;
            Open();
            status = ResourceStatus.EXECUTING;
        }

        /// <summary>
        /// Stop the <see cref="SerialResource"/>
        /// </summary>
        public void Stop()
        {
            status = ResourceStatus.STOPPING;
            Close();
            status = ResourceStatus.STOPPED;
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        /// <summary>
        /// Send a command via serial communication
        /// </summary>
        /// <param name="command">The command to send</param>
        public void Send(string command)
        {
            WriteLine(command);
        }

        /// <summary>
        /// Send a command via serial communication and returns the response
        /// </summary>
        /// <param name="command">The command to send</param>
        /// <param name="value">The response</param>
        public void Send(string command, out string value)
        {
            WriteLine(command);
            value = ReadLine();
        }

        /// <summary>
        /// Return a description of the object
        /// See also <see cref="object.ToString()"/>
        /// </summary>
        /// <returns>The description of the object</returns>
        override
        public string ToString()
        {
            string description = PortName;

            return description;
        }
    }
}
