using Diagnostic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
        private TcpListener listener;
        private CancellationTokenSource tokenSource;
        private CancellationToken token;
        private NetworkStream stream;
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
            string ipAddress = string.Empty;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                ipAddress = endPoint.Address.ToString();
            }

            InitializeVariables(ipAddress, port, encoding);
        }

        /// <summary>
        /// Create a new instance of <see cref="TcpServer"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="ipAddress">The ip address</param>
        /// <param name="port">The port</param>
        /// <param name="encoding">The <see cref="Encoding"/></param>
        public TcpServer(string code, string ipAddress, int port, Encoding encoding) : base(code)
            => InitializeVariables(ipAddress, port, encoding);

        /// <summary>
        /// Initialize the variables
        /// </summary>
        /// <param name="ipAddress">The ip address</param>
        /// <param name="port">The port</param>
        /// <param name="encoding">The <see cref="Encoding"/></param>
        private void InitializeVariables(string ipAddress, int port, Encoding encoding)
        {
            IpAddress = ipAddress.CompareTo("localhost") == 0 ? "127.0.0.1" : ipAddress;
            Port = port;
            StreamInput = new StreamInput($"{Code}.StreamInput", encoding);

            listener = new TcpListener(IPAddress.Parse(IpAddress), Port);
        }

        #endregion Constructors

        public override async Task Restart()
        {
            Stop();
            await Restart();
        }

        public override async Task Start()
        {
            Status.Value = ResourceStatus.Starting;

            try
            {
                listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                if (token == null)
                    token = new CancellationToken();

                tokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
                token = tokenSource.Token;

                listener.Start();
                isListening = true;

                Status.Value = ResourceStatus.Executing;

                await Logger.InfoAsync("Waiting for client to connect...");
                TcpClient client = await listener.AcceptTcpClientAsync();
                await Logger.WarnAsync("Client connected");

                byte[] buffer = new byte[1024];
                List<byte> fullBuffer = new List<byte>();
                int bytesRead = 0;
                while (!token.IsCancellationRequested)
                {
                    stream = client.GetStream();

                    if (stream.CanRead)
                    {
                        do
                        {
                            if (stream.DataAvailable)
                                bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                            else
                                bytesRead = 0;

                            for (int i = 0; i < bytesRead; i++)
                                fullBuffer.Add(buffer[i]);
                        }
                        while (bytesRead > 0);

                        if (fullBuffer.Count > 0)
                            StreamInput.Value = fullBuffer.ToArray();
                    }
                    else
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(1000d));
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public override void Stop()
        {
            Status.Value = ResourceStatus.Stopping;

            try
            {
                isListening = false;

                if (tokenSource != null)
                    tokenSource.Cancel();

                listener.Stop();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void Dispose() => Stop();
    }
}