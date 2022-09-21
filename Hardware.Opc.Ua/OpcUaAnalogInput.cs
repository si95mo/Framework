using System.Threading.Tasks;

namespace Hardware.Opc.Ua
{
    public class OpcUaAnalogInput : OpcUaAnalogChannel
    {
        private int pollingInterval;
        private Task pollingTask;

        public int PollingInterval
        {
            get => pollingInterval;
            set => pollingInterval = value;
        }

        /// <summary>
        /// Create a new instance of <see cref="OpcUaAnalogInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="namespaceConfiguration">The namespace configuration</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        /// <param name="pollingInterval">The polling interval (in milliseconds)</param>
        public OpcUaAnalogInput(string code, string namespaceConfiguration, IResource resource, string measureUnit = "", string format = "0.000", int pollingInterval = 100)
            : base(code, namespaceConfiguration, resource)
        {
            this.pollingInterval = pollingInterval;

            pollingTask = new Task(async () =>
                {
                    while (true)
                    {
                        await (resource as OpcUaResource).Receive(code);
                        await Task.Delay(pollingInterval);
                    }
                }
            );
            pollingTask.Start();
        }
    }
}