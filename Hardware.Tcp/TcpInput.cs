using Hardware.Resources;
using System.Threading.Tasks;

namespace Hardware.Tcp
{
    /// <summary>
    /// An input channel for tcp communication
    /// </summary>
    public class TcpInput : Channel<string>, ITcpChannel
    {
        private string request;
        private string response;
        private IResource resource;
        private int pollingInterval;
        private Task pollingTask;

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
        /// The polling interval (in milliseconds)
        /// </summary>
        public int PollingInterval
        { get => pollingInterval; set => pollingInterval = value; }

        /// <summary>
        /// The polling <see cref="Task"/>
        /// </summary>
        public Task PollingTask => pollingTask;

        /// <summary>
        /// Create a new instance of <see cref="TcpInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="request">The command to send</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="pollingInterval">The polling interval (in milliseconds)</param>
        public TcpInput(string code, string request, IResource resource, int pollingInterval = 100) : base(code)
        {
            this.request = request;
            this.resource = resource;

            response = "";
            value = "";

            pollingTask = new Task(async () =>
                {
                    while (true)
                    {
                        (resource as TcpResource).SendAndReceive(request, out response);
                        value = response;

                        await Task.Delay(pollingInterval);
                    }
                }
            );
            pollingTask.Start();
        }
    }
}