using Core;
using Hardware.Resources;

namespace Hardware.Tcp
{
    /// <summary>
    /// Basic prototype for a tcp channel
    /// </summary>
    public interface ITcpChannel : IProperty<string>
    {
        /// <summary>
        /// The request to send using <see cref="TcpResource"/>
        /// </summary>
        string Request
        { get; set; }

        /// <summary>
        ///
        /// The response received using <see cref="TcpResource"/>
        /// </summary>
        string Response
        { get; set; }

        /// <summary>
        /// The serial <see cref="IResource"/>
        /// </summary>
        IResource Resource { get; set; }
    }
}