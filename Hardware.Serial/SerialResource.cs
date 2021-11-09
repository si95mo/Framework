using Core;
using Core.DataStructures;
using Core.Parameters;
using Diagnostic;
using System;
using System.IO.Ports;
using System.Threading.Tasks;

namespace Hardware.Resources
{
    /// <summary>
    /// Handle the data received in the relative
    /// <see cref="SerialDataReceivedEventHandler"/>
    /// </summary>
    public class DataReceivedArgs : EventArgs
    {
        /// <summary>
        /// The data received
        /// </summary>
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
        private EnumParameter<ResourceStatus> status;
        private IFailure failure;

        private object sendLock = new object();
        private object sendAndReceiveLock = new object();
        private object receiveLock = new object();

        private object objectLock = new object();

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
        public EnumParameter<ResourceStatus> Status
        {
            get => status;
            protected set => status = value;
        }

        /// <summary>
        /// The <see cref="SerialResource"/> last <see cref="IFailure"/>
        /// </summary>
        public IFailure LastFailure => failure;

        /// <summary>
        /// The <see cref="SerialResource"/> <see cref="System.Type"/>
        /// </summary>
        public Type Type => GetType();

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

        /// <summary>
        /// The data received event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void DataReceivedEventHandler(object sender, DataReceivedArgs e);

        /// <summary>
        /// The event handler for the data received
        /// </summary>
        public new event EventHandler<DataReceivedArgs> DataReceived;

        /// <summary>
        /// The on data received event handler
        /// </summary>
        /// <param name="data"></param>
        public virtual void OnDataReceived(byte[] data)
        {
            DataReceived?.Invoke(this, new DataReceivedArgs { Data = data });
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
            channels = new Bag<IChannel>();
            status = new EnumParameter<ResourceStatus>(nameof(status));

            ErrorReceived += SerialResource_ErrorReceived;
            Status.ValueChanged += Status_ValueChanged;
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
            int dataBits = 8, StopBits stopBits = StopBits.One) : this(code)
        {
            PortName = portName;
            BaudRate = baudRate;
            DataBits = dataBits;
            Parity = parity;
            StopBits = stopBits;
            Handshake = Handshake.None;
            DtrEnable = true;
            NewLine = Environment.NewLine;
            ReceivedBytesThreshold = 1024;
        }

        /// <summary>
        /// Handles error in the serial communication
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="SerialErrorReceivedEventArgs"/></param>
        private void SerialResource_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            Status.Value = ResourceStatus.Failure;

            SerialError error = e.EventType;

            failure.Description = error.ToString();
            failure.Timestamp = DateTime.Now;

            Logger.Log(failure.Description, Severity.Warn);
        }

        private void Status_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            switch (status.Value)
            {
                case ResourceStatus.Starting:
                    Logger.Log($"{code} starting", Severity.Info);
                    break;

                case ResourceStatus.Executing:
                    Logger.Log($"{code} executing", Severity.Info);
                    break;

                case ResourceStatus.Stopping:
                    Logger.Log($"{code} stopping", Severity.Info);
                    break;

                case ResourceStatus.Stopped:
                    Logger.Log($"{code} stopped", Severity.Info);
                    break;

                case ResourceStatus.Failure:
                    Logger.Log($"{code} failure", Severity.Info);
                    break;
            }
        }

        private void ContinuousRead()
        {
            byte[] buffer = new byte[4096];
            void kickoffRead()
            {
                if (IsOpen)
                {
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
                }
            }

            kickoffRead();
        }

        /// <summary>
        /// Start the <see cref="SerialResource"/>
        /// </summary>
        public async Task Start()
        {
            try
            {
                failure.Clear();

                Status.Value = ResourceStatus.Starting;
                await Task.Run(() => Open());
                Status.Value = IsOpen ? ResourceStatus.Executing : ResourceStatus.Failure;

                if (status.Value == ResourceStatus.Failure)
                    failure = new Failure("Error occurred while opening the port!", DateTime.Now);
                else
                    ContinuousRead();
            }
            catch (Exception ex)
            {
                Status.Value = ResourceStatus.Failure;
                failure = new Failure(ex.Message);

                Logger.Log(ex);
            }
        }

        /// <summary>
        /// Flush the serial buffer
        /// </summary>
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
            Status.Value = ResourceStatus.Stopping;
            Close();
            Status.Value = !IsOpen ? ResourceStatus.Stopped : ResourceStatus.Failure;

            if (status.Value == ResourceStatus.Failure)
                failure = new Failure("Error occurred while closing the port!", DateTime.Now);
        }

        /// <summary>
        /// Restart the <see cref="SerialResource"/>
        /// </summary>
        public async Task Restart()
        {
            Stop();
            await Start();
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
                    data = ReadLine();
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
        public override string ToString()
        {
            string description = PortName;
            return description;
        }
    }
}