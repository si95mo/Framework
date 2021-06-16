using Core;
using Diagnostic;
using System;
using System.IO.Ports;

namespace Hardware.Resources
{
    /// <summary>
    /// Implement a resource that communicates via the serial protocol.
    /// See also <see cref="IResource"/>
    /// </summary>
    public class SerialResource : SerialPort, IResource
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
        /// Create a new instance of <see cref="SerialResource"/>
        /// </summary>
        public SerialResource() : base()
        { }

        /// <summary>
        /// Create a new instance of <see cref="SerialResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="portName">The port name</param>
        /// <param name="baudRate">The baud rate</param>
        /// <param name="parity">The parity type</param>
        /// <param name="dataBits">Data bits number</param>
        /// <param name="stopbits">Stop bit type</param>
        public SerialResource(string code, string portName, int baudRate = 9600, Parity parity = Parity.None,
            int dataBits = 8, StopBits stopbits = StopBits.One) 
            : base(portName, baudRate, parity, dataBits, stopbits)
        {
            this.code = code;
            failure = new Failure();

            ErrorReceived += SerialResource_ErrorReceived;
        }

        /// <summary>
        /// Handles error in the serial communication
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="SerialErrorReceivedEventArgs"/></param>
        private void SerialResource_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            status = ResourceStatus.Failure;

            SerialError error = e.EventType;
           
            failure.Description = error.ToString();
            failure.Timestamp = DateTime.Now;

            Logger.Log(failure.Description, SeverityType.Debug);
        }

        /// <summary>
        /// Start the <see cref="SerialResource"/>
        /// </summary>
        public void Start()
        {
            failure.Clear();

            status = ResourceStatus.Starting;
            Open();
            status = IsOpen ? ResourceStatus.Executing : ResourceStatus.Failure;

            if (status == ResourceStatus.Failure)
                failure = new Failure("Error occurred while opening the port!", DateTime.Now);
        }

        /// <summary>
        /// Stop the <see cref="SerialResource"/>
        /// </summary>
        public void Stop()
        {
            status = ResourceStatus.Stopping;
            Close();
            status = !IsOpen ? ResourceStatus.Stopped : ResourceStatus.Failure;

            if (status == ResourceStatus.Failure)
                failure = new Failure("Error occurred while closing the port!", DateTime.Now);
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        /// <summary>
        /// Send a command via serial protocol
        /// </summary>
        /// <param name="command">The command to send</param>
        public void Send(string command)
        {
            WriteLine(command);
        }

        /// <summary>
        /// Send a command via serial protocol and receive the response
        /// </summary>
        /// <param name="command">The command to send</param>
        /// <param name="response">The response</param>
        public void Send(string command, out string response)
        {
            WriteLine(command);
            response = ReadLine();
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
