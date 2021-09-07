using Core;
using Core.DataStructures;
using Diagnostic;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Hardware.Resources
{
    /// <summary>
    /// Implement a resource that communicates via the tcp protocol.
    /// See also <see cref="IResource"/>
    /// </summary>
    public class TcpResource : Resource
    {
        protected string ipAddress;
        protected int port;

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

        /// <summary>
        /// Create a new instance of <see cref="TcpResource"/>
        /// </summary>
        public TcpResource() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Create a new instance of <see cref="TcpResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        public TcpResource(string code)
        {
            this.code = code;
            ipAddress = "";
            port = 0;
            failure = new Failure();
            channels = new Bag<IChannel>();

            tcp = new TcpClient();

            status = ResourceStatus.Stopped;
        }

        /// <summary>
        /// Create a new instance of <see cref="TcpResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="ipAddress">The ip address</param>
        /// <param name="port">The port number</param>
        /// <param name="timeout">The timeout (in milliseconds)</param>
        public TcpResource(string code, string ipAddress, int port, int timeout = 5000)
        {
            try
            {
                this.code = code;
                this.ipAddress = ipAddress;
                this.port = port;
                failure = new Failure();
                channels = new Bag<IChannel>();

                tcp = new TcpClient();
                tcp.ReceiveTimeout = timeout;
                tcp.SendTimeout = timeout;

                status = ResourceStatus.Stopped;
            }
            catch (Exception ex)
            {
                failure = new Failure(ex.Message);
                Logger.Log(ex);
            }
        }

        /// <summary>
        /// Send a command via tcp protocol, without receiving a response
        /// </summary>
        /// <param name="request">The http request to send</param>
        public void Send(string request)
        {
            try
            {
                // Request
                var requestData = Encoding.UTF8.GetBytes(request);
                tcp.Client.Send(requestData);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        private string Receive()
        {
            string response = "";

            try
            {
                byte[] responseData = new byte[1024];
                int lengthOfResponse = tcp.Client.Receive(responseData);

                response = Encoding.UTF8.GetString(responseData, 0, lengthOfResponse);

                return response;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return response;
            }
        }

        /// <summary>
        /// Send a command via serial protocol and receive the response
        /// </summary>
        /// <param name="request">The request to send</param>
        /// <param name="response">The response received</param>
        public void SendAndReceive(string request, out string response)
        {
            response = "";

            try
            {
                Send(request);
                response = Receive();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        /// <summary>
        /// Restart the <see cref="TcpResource"/>
        /// </summary>
        public override async void Restart()
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
            Status = ResourceStatus.Starting;
            tcp = new TcpClient();

            try
            {
                await tcp.ConnectAsync(ipAddress, port);

                if (TestConnection())
                    Status = ResourceStatus.Executing;
                else
                    Status = ResourceStatus.Failure;

                if (status == ResourceStatus.Failure)
                    failure = new Failure("Error occurred while opening the port!", DateTime.Now);
            }
            catch (Exception ex)
            {
                Status = ResourceStatus.Failure;
                failure = new Failure(ex.Message, DateTime.Now);
            }
        }

        /// <summary>
        /// Stop the <see cref="TcpResource"/>
        /// </summary>
        public override void Stop()
        {
            status = ResourceStatus.Stopping;
            tcp.Close();
            status = ResourceStatus.Stopped;

            failure = new Failure("Error occurred while closing the port!", DateTime.Now);
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
            catch(Exception ex)
            {
                Logger.Log(ex);
                return result;
            }
        }
    }
}