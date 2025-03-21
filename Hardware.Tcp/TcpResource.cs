﻿using Core;
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
                Status.Value = ResourceStatus.Failure;
                Logger.Log(ex);
            }
        }

        /// <summary>
        /// Receive data via the <see cref="TcpResource"/>
        /// </summary>
        /// <returns></returns>
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
                HandleException(ex);

                return response;
            }
        }

        /// <summary>
        /// Send a command via the <see cref="TcpResource"/> and receive the response
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