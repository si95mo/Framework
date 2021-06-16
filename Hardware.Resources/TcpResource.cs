using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Hardware.Resources
{
    /// <summary>
    /// Implement a resource that communicates via the tcp protocol.
    /// See also <see cref="IResource"/>
    /// </summary>
    class TcpResource : IResource
    {
        private string code;
        private ResourceStatus status;
        private IFailure failure;

        private string ipAddress;
        private int port;

        private TcpClient tcp;

        /// <summary>
        /// The <see cref="TcpResource"/> code
        /// </summary>
        public string Code => code;

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

        public int Port => port;

        /// <summary>
        /// Create a new instance of <see cref="TcpResource"/>
        /// </summary>
        public TcpResource()
        {
            ipAddress = "";
            port = 0;

            tcp = new TcpClient();

            status = ResourceStatus.STOPPED;
        }

        /// <summary>
        /// Create a new instance of <see cref="TcpResource"/>
        /// </summary>
        /// <param name="ipAddress">The ip address</param>
        /// <param name="port">The port number</param>
        public TcpResource(string ipAddress, int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;

            tcp = new TcpClient();

            status = ResourceStatus.STOPPED;
        }

        /// <summary>
        /// Send a commend via tcp protocol
        /// </summary>
        /// <param name="request"> The http request to send </param>
        /// <returns> The response to the request from the server </returns>
        public void Send(string request)
        {
            // Request
            var requestData = Encoding.UTF8.GetBytes(request);
            tcp.Client.Send(requestData);

            // Response
            byte[] responseData = new byte[1024];
            int lengthOfResponse = tcp.Client.Receive(responseData);
        }

        /// <summary>
        /// Send a command via serial protocol and receive the response
        /// </summary>
        /// <param name="command">The command to send</param>
        /// <param name="response">The response</param>
        public void Send(string request, out string response)
        {
            // Request
            var requestData = Encoding.UTF8.GetBytes(request);
            tcp.Client.Send(requestData);

            // Response
            byte[] responseData = new byte[1024];
            int lengthOfResponse = tcp.Client.Receive(responseData);

            response = Encoding.UTF8.GetString(responseData, 0, lengthOfResponse);
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

            status = ResourceStatus.STARTING;
            tcp.Connect(ipAddress, port);
            status = IsOpen ? ResourceStatus.EXECUTING : ResourceStatus.FAILURE;

            if (status == ResourceStatus.FAILURE)
                failure = new Failure("Error occurred while opening the port!", DateTime.Now);
        }

        /// <summary>
        /// Stop the <see cref="TcpResource"/>
        /// </summary>
        public void Stop()
        {
            status = ResourceStatus.STOPPING;
            tcp.Close();
            status = !IsOpen ? ResourceStatus.STOPPED : ResourceStatus.FAILURE;

            if (status == ResourceStatus.FAILURE)
                failure = new Failure("Error occurred while closing the port!", DateTime.Now);
        }
    }
}
