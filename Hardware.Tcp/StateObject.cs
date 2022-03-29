using System.Net.Sockets;
using System.Text;

namespace Hardware.Tcp
{
    /// <summary>
    /// State object class for receiving from a remote device
    /// </summary>
    internal class StateObject
    {
        /// <summary>
        /// The client <see cref="System.Net.Sockets.Socket"/>
        /// </summary>
        public Socket Socket = null;

        /// <summary>
        /// Receive buffer size
        /// </summary>
        public const int BufferSize = 256;

        /// <summary>
        /// Receive buffer
        /// </summary>
        public byte[] Buffer = new byte[BufferSize];

        /// <summary>
        /// Received data string
        /// </summary>
        public StringBuilder StringBuilder = new StringBuilder();

        public ITcpChannel TcpChannel;
    }
}