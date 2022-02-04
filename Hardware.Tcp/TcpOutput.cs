using Core;
using Hardware.Resources;

namespace Hardware.Tcp
{
    /// <summary>
    /// An output channel for tcp communication
    /// </summary>
    public class TcpOutput : Channel<string>, ITcpChannel
    {
        private string request;
        private string response;
        private IResource resource;

        /// <summary>
        /// The request to send
        /// </summary>
        public string Request
        { get => request; set => request = value; }

        /// <summary>
        /// The received response
        /// </summary>
        public string Response => response;

        /// <summary>
        /// The <see cref="IResource"/>
        /// </summary>
        public IResource Resource
        { get => resource; set => resource = value; }

        /// <summary>
        /// Create a new instance of <see cref="TcpOutput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="request">The request to send</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        public TcpOutput(string code, string request, IResource resource) : base(code)
        {
            this.request = request;
            this.resource = resource;

            response = "";
            value = "";

            ValueChanged += TcpOutput_ValueChanged;
        }

        private void TcpOutput_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            request = value;
            (resource as TcpResource).Send(request);
        }
    }
}