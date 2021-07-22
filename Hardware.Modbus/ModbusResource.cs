using Core;
using Core.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Hardware.Modbus
{
    public class ModbusResource : IResource
    {
        private string code;
        private ResourceStatus status;
        private IFailure failure;
        private Bag<IChannel> channels;
        private TcpClient tcp;

        private string ipAddress;
        private int port;

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

            

            if (status == ResourceStatus.Failure)
                failure = new Failure("Error occurred while opening the port!", DateTime.Now);
        }

        /// <summary>
        /// Stop the <see cref="TcpResource"/>
        /// </summary>
        public void Stop()
        {
            status = ResourceStatus.Stopping;

            

            if (status == ResourceStatus.Failure)
                failure = new Failure("Error occurred while closing the port!", DateTime.Now);
        }
    }
}
