using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Hardware.Tcp
{
    /// <summary>
    /// Handle the received data event arguments
    /// </summary>
    public class DataReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// The <see cref="NetworkStream"/>
        /// </summary>
        public NetworkStream Stream { get; private set; }

        /// <summary>
        /// Create a new instance of <see cref="DataReceivedEventArgs"/>
        /// </summary>
        /// <param name="stream"></param>
        public DataReceivedEventArgs(NetworkStream stream)
        {
            Stream = stream;
        }
    }

    /// <summary>
    /// Implement a TCP server
    /// </summary>
    public class TcpServer : Resource, IDisposable
    {
        private readonly TcpListener listener;
        private volatile bool isListening = false;

        #region Public properties

        public override bool IsOpen => Status.Value == ResourceStatus.Executing;

        /// <summary>
        /// The <see cref="TcpServer"/> ip address
        /// </summary>
        public string IpAddress { get; private set; }

        /// <summary>
        /// The <see cref="TcpServer"/> port
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// The <see cref="StreamInput"/>
        /// </summary>
        public StreamInput StreamInput { get; private set; }

        public event EventHandler<DataReceivedEventArgs> DataReceived;

        #endregion Public properties

        #region Constructors

        /// <summary>
        /// Create a new instance of <see cref="TcpServer"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="port">The port</param>
        /// <param name="encoding">The <see cref="Encoding"/></param>
        public TcpServer(string code, int port, Encoding encoding) : base(code)
        {
            // Get the local ip address
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                IpAddress = endPoint.Address.ToString();
            }

            Port = port;
            StreamInput = new StreamInput($"{Code}.StreamInput", encoding);
        }

        /// <summary>
        /// Create a new instance of <see cref="TcpServer"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="ipAddress">The ip address</param>
        /// <param name="port">The port</param>
        /// <param name="encoding">The <see cref="Encoding"/></param>
        public TcpServer(string code, string ipAddress, int port, Encoding encoding) : base(code)
        {
            IpAddress = ipAddress;
            Port = port;
            StreamInput = new StreamInput($"{Code}.StreamInput", encoding);
        }

        #endregion Constructors

        public override async Task Restart()
        {
            Stop();
            await Restart();
        }

        public override Task Start()
        {
            Status.Value = ResourceStatus.Starting;

            try
            {
                listener.Start();
                isListening = true;

                WaitForClientConnection();

                Status.Value = ResourceStatus.Executing;
            }
            catch(Exception ex)
            {
                HandleException(ex);
            }

            return Task.CompletedTask;
        }

        public override void Stop()
        {
            Status.Value = ResourceStatus.Stopping;

            try
            {
                isListening = false;
                listener.Stop();
            }
            catch(Exception ex)
            {
                HandleException(ex);
            }
        }

        public void Dispose() => Stop();

        #region Helper methods

        /// <summary>
        /// Wait for a client connection
        /// </summary>
        private void WaitForClientConnection()
            => listener.BeginAcceptTcpClient(HandleClientConnection, listener);

        /// <summary>
        /// Handle a new client connection
        /// </summary>
        /// <param name="result"></param>
        private void HandleClientConnection(IAsyncResult result)
        {
            if (!isListening)
            {
                return;
            }

            var server = result.AsyncState as TcpListener;
            var client = listener.EndAcceptTcpClient(result);

            WaitForClientConnection();

            NetworkStream stream = client.GetStream();
            DataReceived.Invoke(this, new DataReceivedEventArgs(stream));

            // Get the data from the network stream and then set it to the StreamInput
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            StreamInput.Value = buffer;
        }

        #endregion Helper methods
    }
}
