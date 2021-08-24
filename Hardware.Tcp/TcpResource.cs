using Core;
using Core.DataStructures;
using Diagnostic;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Hardware.Resources
{
    /// <summary>
    /// Implement a resource that communicates via the tcp protocol.
    /// See also <see cref="IResource"/>
    /// </summary>
    public class TcpResource : IResource
    {
        protected string code;
        protected ResourceStatus status;
        protected IFailure failure;
        protected Bag<IChannel> channels;

        protected string ipAddress;
        protected int port;

        protected TcpClient tcp;

        /// <summary>
        /// The <see cref="TcpResource"/> code
        /// </summary>
        public string Code => code;

        /// <summary>
        /// The <see cref="TcpResource"/> <see cref="Bag{IProperty}"/> of
        /// <see cref="IChannel"/>;
        /// </summary>
        public Bag<IChannel> Channels => channels;

        /// <summary>
        /// The <see cref="TcpResource"/> status
        /// </summary>
        public ResourceStatus Status => status;

        /// <summary>
        /// The last <see cref="IFailure"/>
        /// </summary>
        public IFailure LastFailure => failure;

        /// <summary>
        /// The <see cref="TcpResource"/> operating state
        /// </summary>
        public bool IsOpen => tcp.Connected;

        /// <summary>
        /// The <see cref="TcpResource"/> ip address
        /// </summary>
        public string IpAddress => ipAddress;

        public Type Type => this.GetType();

        /// <summary>
        /// The <see cref="TcpResource"/> value as <see cref="object"/>
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
            catch(Exception ex)
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
            catch(Exception ex)
            {
                Logger.Log(ex);
            }
        }

        /// <summary>
        /// Restart the <see cref="TcpResource"/>
        /// </summary>
        public void Restart()
        {
            Stop();
            Start();
        }

        /// <summary>
        /// Start the <see cref="TcpResource"/>
        /// </summary>
        public void Start()
        {
            failure.Clear();
            status = ResourceStatus.Starting;

            tcp.Connect(ipAddress, port);

            if (TestConnection())
                status = ResourceStatus.Executing;
            else
                status = ResourceStatus.Failure;

            if (status == ResourceStatus.Failure)
                failure = new Failure("Error occurred while opening the port!", DateTime.Now);
        }

        /// <summary>
        /// Stop the <see cref="TcpResource"/>
        /// </summary>
        public void Stop()
        {
            status = ResourceStatus.Stopping;

            tcp.Close();

            if (TestConnection())
                status = ResourceStatus.Stopped;

            else
                status = ResourceStatus.Failure;

            if (status == ResourceStatus.Failure)
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
    }
}