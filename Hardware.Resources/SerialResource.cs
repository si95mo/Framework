using Core;
using Core.DataStructures;
using Diagnostic;
using System;
using System.IO.Ports;

namespace Hardware.Resources
{
    public class DataReceivedArgs : EventArgs
    {
        public byte[] Data { get; set; }
    }

    /// <summary>
    /// Implement a resource that communicates via the serial protocol.
    /// See also <see cref="IResource"/>
    /// </summary>
    public class SerialResource : SerialPort, IResource
    {
        private readonly string code;
        private readonly Bag<IChannel> channels;
        private ResourceStatus status;
        private IFailure failure;

        private object sendLock = new object();
        private object sendAndReceiveLock = new object();
        private object receiveLock = new object();

        /// <summary>
        /// The <see cref="SerialResource"/> code
        /// </summary>
        public string Code => code;

        /// <summary>
        /// The <see cref="SerialResource"/> <see cref="Bag{IProperty}"/>
        /// of <see cref="IChannel"/>
        /// </summary>
        public Bag<IChannel> Channels => channels;

        /// <summary>
        /// The <see cref="SerialResource"/> status
        /// </summary>
        public ResourceStatus Status => status;

        /// <summary>
        /// The <see cref="SerialResource"/> last <see cref="IFailure"/>
        /// </summary>
        public IFailure LastFailure => failure;

        public Type Type => this.GetType();

        /// <summary>
        /// The <see cref="SerialResource"/> value as <see cref="object"/>
        /// </summary>
        public object ValueAsObject
        {
            get => code;
            set
            {
                _ = ValueAsObject;
            }
        }

        public delegate void DataReceivedEventHandler(object sender, DataReceivedArgs e);
        public new event EventHandler<DataReceivedArgs> DataReceived;

        public virtual void OnDataReceived(byte[] data)
        {
            var handler = DataReceived;
            if (handler != null)
            {
                handler(this, new DataReceivedArgs { Data = data });
            }
        }

        /// <summary>
        /// Create a new instance of <see cref="SerialResource"/>
        /// </summary>
        public SerialResource() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Create a new instance of <see cref="SerialResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        public SerialResource(string code) : base()
        {
            this.code = code;
            failure = new Failure();

            ErrorReceived += SerialResource_ErrorReceived;
        }

        /// <summary>
        /// Create a new instance of <see cref="SerialResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="portName">The port name</param>
        /// <param name="baudRate">The baud rate</param>
        /// <param name="parity">The parity type</param>
        /// <param name="dataBits">Data bits number</param>
        /// <param name="stopBits">Stop bit type</param>
        public SerialResource(string code, string portName, int baudRate = 9600, Parity parity = Parity.None,
            int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            this.code = code;
            failure = new Failure();

            PortName = portName;
            BaudRate = baudRate;
            DataBits = dataBits;
            Parity = parity;
            StopBits = stopBits;
            Handshake = Handshake.None;
            DtrEnable = true;
            NewLine = Environment.NewLine;
            ReceivedBytesThreshold = 1024;

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

            Logger.Log(failure.Description, Severity.Warn);
        }

        private void ContinuousRead()
        {
            byte[] buffer = new byte[4096];
            Action kickoffRead = null;
            kickoffRead = () => 
                BaseStream.BeginRead(
                    buffer, 
                    0, 
                    buffer.Length, 
                    delegate (IAsyncResult ar)
                    {
                        try
                        {
                            int count = BaseStream.EndRead(ar);
                            byte[] dst = new byte[count];
                            Buffer.BlockCopy(buffer, 0, dst, 0, count);
                            OnDataReceived(dst);
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(ex);
                        }
                        kickoffRead();
                    }, 
                    null
                ); 
            kickoffRead();
        }

        /// <summary>
        /// Start the <see cref="SerialResource"/>
        /// </summary>
        public void Start()
        {
            try
            {
                failure.Clear();

                status = ResourceStatus.Starting;
                Open();
                status = IsOpen ? ResourceStatus.Executing : ResourceStatus.Failure;

                if (status == ResourceStatus.Failure)
                    failure = new Failure("Error occurred while opening the port!", DateTime.Now);
                else
                    ContinuousRead();
            }
            catch (Exception ex)
            {
                status = ResourceStatus.Failure;
                failure = new Failure(ex.Message);

                Logger.Log(ex);
            }
        }

        public void Flush()
        {
            DiscardInBuffer();
            DiscardOutBuffer();
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
            lock (sendLock)
            {
                WriteLine(command);
                Flush();
            }
        }

        /// <summary>
        /// Send a command via serial protocol and receive the response
        /// </summary>
        /// <param name="command">The command to send</param>
        /// <param name="response">The response</param>
        public void Send(string command, out string response)
        {
            lock (sendAndReceiveLock)
            {
                WriteLine(command);
                response = ReadLine();
                Flush();
            }
        }

        /// <summary>
        /// Receive data via serial protocol
        /// </summary>
        /// <returns>The data received</returns>
        public string Receive()
        {
            string data = "";

            if (IsOpen)
            {
                lock (receiveLock)
                {
                    data = Data
                    Flush();
                }
            }

            return data;
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