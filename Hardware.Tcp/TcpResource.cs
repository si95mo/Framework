using Core;
using Core.DataStructures;
using Hardware.Tcp;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hardware.Resources
{
    /// <summary>
    /// Implement a resource that communicates via the tcp protocol.
    /// See also <see cref="IResource"/>
    /// </summary>
    public class TcpResource : Resource
    {
        /// <summary>
        /// The ip address
        /// </summary>
        protected string ipAddress;

        /// <summary>
        /// The port number
        /// </summary>
        protected int port;

        /// <summary>
        /// The underling <see cref="TcpClient"/>
        /// </summary>
        protected TcpClient tcp;

        /// <summary>
        /// The <see cref="TcpResource"/> operating state
        /// </summary>
        public override bool IsOpen => tcp.Connected;

        /// <summary>
        /// The <see cref="TcpResource"/> ip address
        /// </summary>
        public string IpAddress => ipAddress;

        /// <summary>
        /// The <see cref="TcpResource"/> port number
        /// </summary>
        public int Port => port;

        private IPGlobalProperties ipProperties;
        private TcpConnectionInformation[] tcpConnections;

        private ManualResetEventSlim sendDone, receiveDone;

        /// <summary>
        /// Create a new instance of <see cref="TcpResource"/>
        /// </summary>
        public TcpResource() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Create a new instance of <see cref="TcpResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        public TcpResource(string code) : base(code)
        {
            ipAddress = "";
            port = 0;
            failure = new Failure();
            channels = new Bag<IChannel>();

            tcp = new TcpClient();

            sendDone = new ManualResetEventSlim(false);
            receiveDone = new ManualResetEventSlim(false);

            status.Value = ResourceStatus.Stopped;
        }

        /// <summary>
        /// Create a new instance of <see cref="TcpResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="ipAddress">The ip address</param>
        /// <param name="port">The port number</param>
        /// <param name="timeout">The timeout (in milliseconds)</param>
        public TcpResource(string code, string ipAddress, int port, int timeout = 5000) : base(code)
        {
            try
            {
                this.ipAddress = ipAddress;
                this.port = port;
                failure = new Failure();
                channels = new Bag<IChannel>();

                tcp = new TcpClient
                {
                    ReceiveTimeout = timeout,
                    SendTimeout = timeout
                };

                sendDone = new ManualResetEventSlim(false);
                receiveDone = new ManualResetEventSlim(false);

                status.Value = ResourceStatus.Stopped;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Send a command via the <see cref="TcpResource"/>, without receiving a response
        /// </summary>
        /// <param name="channel">The <see cref="ITcpChannel"/></param>
        public void Send(ITcpChannel channel)
        {
            // Convert the string data to byte data using ASCII encoding
            byte[] byteData = Encoding.ASCII.GetBytes(channel.Request);

            // Begin sending the data to the remote device
            tcp.Client.BeginSend(
                byteData,
                0,
                byteData.Length,
                0,
                new AsyncCallback(SendCallback),
                tcp.Client
            );
        }

        private void SendCallback(IAsyncResult asyncResult)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)asyncResult.AsyncState;

                // Complete sending the data to the remote device
                int bytesSent = client.EndSend(asyncResult);

                // Signal that all bytes have been sent
                sendDone.Set();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Receive data via the <see cref="TcpResource"/>
        /// </summary>
        /// <returns></returns>
        private void Receive(ITcpChannel channel)
        {
            try
            {
                // Create the state object
                StateObject state = new StateObject();
                state.Socket = tcp.Client;
                state.TcpChannel = channel;

                // Begin receiving the data from the remote device
                tcp.Client.BeginReceive(
                    state.Buffer,
                    0,
                    StateObject.BufferSize,
                    0,
                    new AsyncCallback(ReceiveCallback),
                    state
                );
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            try
            {
                // Retrieve the state object and the client socket
                // from the asynchronous state object
                StateObject state = (StateObject)asyncResult.AsyncState;
                Socket client = state.Socket;

                // Read data from the remote device
                int bytesRead = client.EndReceive(asyncResult);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far
                    state.StringBuilder.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));

                    if (state.TcpChannel.Value.CompareTo(string.Empty) == 0)
                        state.TcpChannel.Value = state.StringBuilder.ToString();

                    // Get the rest of the data
                    client.BeginReceive(
                        state.Buffer,
                        0,
                        StateObject.BufferSize,
                        0,
                        new AsyncCallback(ReceiveCallback),
                        state
                    );
                }
                else
                {
                    // All the data has arrived; put it in response
                    if (state.StringBuilder.Length > 1)
                        state.TcpChannel.Value = state.StringBuilder.ToString();

                    // Signal that all bytes have been received
                    receiveDone.Set();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Send a command via the <see cref="TcpResource"/> and receive the response
        /// </summary>
        /// <param name="channel">The <see cref="ITcpChannel"/></param>
        public void SendAndReceive(ITcpChannel channel)
        {
            try
            {
                Send(channel);
                Receive(channel);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Restart the <see cref="TcpResource"/>
        /// </summary>
        public override async Task Restart()
        {
            Stop();
            await Start();
        }

        /// <summary>
        /// Start the <see cref="TcpResource"/>
        /// </summary>
        public override async Task Start()
        {
            failure.Clear();
            Status.Value = ResourceStatus.Starting;
            tcp = new TcpClient();

            try
            {
                await tcp.ConnectAsync(ipAddress, port);

                if (TestConnection())
                    Status.Value = ResourceStatus.Executing;
                else
                    Status.Value = ResourceStatus.Failure;

                if (status.Value == ResourceStatus.Failure)
                    failure = new Failure("Error occurred while opening the port!", DateTime.Now);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Stop the <see cref="TcpResource"/>
        /// </summary>
        public override void Stop()
        {
            Status.Value = ResourceStatus.Stopping;
            tcp.Close();
            Status.Value = ResourceStatus.Stopped;
        }

        /// <summary>
        /// Test for active tcp connection ad the
        /// specified <see cref="IpAddress"/> and
        /// <see cref="Port"/>
        /// </summary>
        /// <returns><see langword="true"/> if there is an active connection,
        /// <see langword="false"/> otherwise</returns>
        protected bool TestConnection()
        {
            bool result = false;

            try
            {
                ipProperties = IPGlobalProperties.GetIPGlobalProperties();
                tcpConnections = ipProperties.GetActiveTcpConnections().Where(
                    x =>
                        x.LocalEndPoint.Equals(tcp.Client?.LocalEndPoint) &&
                        x.RemoteEndPoint.Equals(tcp.Client?.RemoteEndPoint)
                ).ToArray();

                if (tcpConnections != null && tcpConnections.Length > 0)
                {
                    TcpState stateOfConnection = tcpConnections.First().State;

                    if (stateOfConnection == TcpState.Established)
                        result = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return result;
            }
        }
    }
}