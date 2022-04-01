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
        private bool useSendAndReceive;
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

                    if (useSendAndReceive)
                        (Resource as TcpResource).SendAndReceive(this);
                    else
                        (Resource as TcpResource).Send(this);
                }
            }
        }

        /// <summary>
        /// The received response
        /// </summary>
        public string Response
        { get => response; set => response = value; }

        /// <summary>
        /// The <see cref="TcpChannel"/> value.
        /// Represent the <see cref="Response"/> if the getter method is used and
        /// the <see cref="Request"/> if the setter is used instead. <br/>
        /// So, if the setter is used, a new request is sent to the underlying <see cref="TcpResource"/>, while,
        /// if the getter method is used, the last retrieved response from the <see cref="TcpResource"/> is returned instead
        /// </summary>
        public override string Value
        { get => Response; set => Request = value; }

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
        public TcpChannel(string code, IResource resource) : this(code, "", resource, 100, false, true)
        { }

        /// <summary>
        /// Create a new instance of <see cref="TcpChannel"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="request">The command to send</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="pollingInterval">The polling interval (in milliseconds)</param>
        /// <param name="usePolling"><see langword="true"/> if polling must be used, <see langword="false"/> for async send/receive instead</param>
        /// <param name="useSendAndReceive">Use a send/receive logic (the request is sent and then the response is awaited if <see langword="true"/>)</param>
        public TcpChannel(string code, string request, IResource resource, int pollingInterval = 100, bool usePolling = true, bool useSendAndReceive = true)
            : base(code)
        {
            this.request = request;
            this.resource = resource;

            this.pollingInterval = pollingInterval;
            this.useSendAndReceive = useSendAndReceive;

            response = "";
            value = "";

            pollingTask = new Task(async () =>
                {
                    while (true)
                    {
                        if (useSendAndReceive)
                            (resource as TcpResource).SendAndReceive(this);
                        else
                            (resource as TcpResource).Send(this);

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