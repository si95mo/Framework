using Hardware.Resources;
using System.Threading.Tasks;

namespace Hardware.Tcp
{
    /// <summary>
    /// An input channel for tcp communication
    /// </summary>
    public class TcpChannel : Channel<string>, ITcpChannel
    {
        private string request;
        private string response;
        private IResource resource;
        private int pollingInterval;
        private Task pollingTask;

        /// <summary>
        /// The request to send (if the actual request is not equal to the previous one,
        /// it is automatically sent when using the setter)
        /// </summary>
        public string Request
        {
            get => request;
            set
            {
                if (request.CompareTo(value) != 0)
                {
                    response = "";
                    request = value;
                    (Resource as TcpResource).SendAndReceive(this);
                }
            }
        }

        /// <summary>
        /// The received response
        /// </summary>
        public string Response
        { get => response; set => response = value; }

        /// <summary>
        /// The <see cref="IResource"/>
        /// </summary>
        public IResource Resource
        { get => resource; set => resource = value; }

        /// <summary>
        /// The polling interval (in milliseconds)
        /// </summary>
        public int PollingInterval
        { get => pollingInterval; set => pollingInterval = value; }

        /// <summary>
        /// The polling <see cref="Task"/>
        /// </summary>
        public Task PollingTask => pollingTask;

        /// <summary>
        /// Create a new instance of <see cref="TcpChannel"/> that use an async send/receive logic
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        public TcpChannel(string code, IResource resource) : this(code, "", resource, 100, false)
        { }

        /// <summary>
        /// Create a new instance of <see cref="TcpChannel"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="request">The command to send</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="pollingInterval">The polling interval (in milliseconds)</param>
        /// <param name="usePolling"><see langword="true"/> if polling must be used, <see langword="false"/> for async send/receive instead</param>
        public TcpChannel(string code, string request, IResource resource, int pollingInterval = 100, bool usePolling = true) : base(code)
        {
            this.request = request;
            this.resource = resource;

            response = "";
            value = "";

            pollingTask = new Task(async () =>
                {
                    while (true)
                    {
                        (resource as TcpResource).SendAndReceive(this);
                        value = response;

                        await Task.Delay(pollingInterval);
                    }
                }
            );

            if (usePolling)
                pollingTask.Start();
        }
    }
}