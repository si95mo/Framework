using Core;
using Diagnostic;
using System;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;

namespace Hardware.Resources
{
    /// <summary>
    /// Implement a resource that communicates via the serial protocol.
    /// See also <see cref="IResource"/>
    /// </summary>
    public class SerialResource : StreamResource
    {
        private SerialPort port;

        public override bool IsOpen => port.IsOpen;

        #region Constructors

        /// <summary>
        /// Create a new instance of <see cref="SerialResource"/>
        /// </summary>
        public SerialResource() : base(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Create a new instance of <see cref="SerialResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="portName">The port name</param>
        /// <param name="encoding">The <see cref="Encoding"/></param>
        /// <param name="terminatorSequence">The terminator sequence in the stream</param>
        public SerialResource(string code, string portName, Encoding encoding, string terminatorSequence) : base(code, encoding, terminatorSequence)
        {
            port = new SerialPort(portName);

            port.ErrorReceived += SerialResource_ErrorReceived;
            Output.ValueChanged += Output_ValueChanged;
        }

        /// <summary>
        /// Create a new instance of <see cref="SerialResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="portName">The port name</param>
        /// <param name="encoding">The <see cref="Encoding"/></param>
        /// <param name="terminatorSequence">The terminator sequence in the stream</param>
        /// <param name="baudRate">The baud rate</param>
        /// <param name="parity">The parity type</param>
        /// <param name="dataBits">Data bits number</param>
        /// <param name="stopBits">Stop bit type</param>
        public SerialResource(string code, string portName, Encoding encoding, string terminatorSequence, int baudRate = 9600, Parity parity = Parity.None,
            int dataBits = 8, StopBits stopBits = StopBits.One) : this(code, portName, encoding, terminatorSequence)
        {
            port.PortName = portName;
            port.BaudRate = baudRate;
            port.DataBits = dataBits;
            port.Parity = parity;
            port.StopBits = stopBits;
            port.Handshake = Handshake.None;
            port.DtrEnable = true;
            port.NewLine = Environment.NewLine;
            port.ReceivedBytesThreshold = 1024;
        }

        #endregion Constructors

        #region Event handlers

        /// <summary>
        /// Handles error in the serial communication
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="SerialErrorReceivedEventArgs"/></param>
        private void SerialResource_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            SerialError error = e.EventType;
            HandleException(error.ToString());
        }

        private void Output_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Send(Output.EncodedValue);
        }

        /// <summary>
        /// The on data received event handler
        /// </summary>
        /// <param name="data"></param>
        public virtual void OnDataReceived(byte[] data)
        {
            FrameDetector.Add(data);
            if (FrameDetector.TryGet(out byte[] frame))
            {
                Input.Value = frame;
            }
        }

        #endregion Event handlers

        #region Public methods

        #region IResource implementation

        /// <summary>
        /// Start the <see cref="SerialResource"/>
        /// </summary>
        public override async Task Start()
        {
            try
            {
                LastFailure.Clear();

                Status.Value = ResourceStatus.Starting;
                await Task.Run(() => port.Open());
                Status.Value = IsOpen ? ResourceStatus.Executing : ResourceStatus.Failure;

                if (Status.Value == ResourceStatus.Failure)
                {
                    HandleException("Error on Start");
                }
                else
                {
                    ContinuousRead();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Stop the <see cref="SerialResource"/>
        /// </summary>
        public override void Stop()
        {
            Status.Value = ResourceStatus.Stopping;
            port.Close();
            Status.Value = !IsOpen ? ResourceStatus.Stopped : ResourceStatus.Failure;

            if (Status.Value == ResourceStatus.Failure)
            {
                HandleException("Error on Stop");
            }
        }

        /// <summary>
        /// Restart the <see cref="SerialResource"/>
        /// </summary>
        public override async Task Restart()
        {
            Stop();
            await Start();
        }

        #endregion IResource implementation

        /// <summary>
        /// Flush the serial buffer
        /// </summary>
        public void Flush()
        {
            port.DiscardInBuffer();
            port.DiscardOutBuffer();
        }

        #endregion Public methods

        #region Private methods

        /// <summary>
        /// Start a continuous read, updating <see cref="Input"/>
        /// </summary>
        private void ContinuousRead()
        {
            byte[] buffer = new byte[4096];
            void kickoffRead()
            {
                if (IsOpen)
                {
                    port.BaseStream.BeginRead(
                        buffer,
                        0,
                        buffer.Length,
                        delegate (IAsyncResult ar)
                        {
                            try
                            {
                                int count = port.BaseStream.EndRead(ar);
                                byte[] destinationBuffer = new byte[count];
                                Buffer.BlockCopy(buffer, 0, destinationBuffer, 0, count);
                                OnDataReceived(destinationBuffer);
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
        /// Send a command via serial protocol
        /// </summary>
        /// <param name="message">The message to send</param>
        private void Send(string message)
        {
            lock (Output)
            {
                port.WriteLine(message);
                Flush();
            }
        }

        #endregion Private methods
    }
}