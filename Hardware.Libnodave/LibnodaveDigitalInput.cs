using Core;
using System;
using System.Threading.Tasks;

namespace Hardware.Libnodave
{
    /// <summary>
    /// Implement a digital input channel for the <see cref="LibnodaveResource"/>
    /// </summary>
    public class LibnodaveDigitalInput : LibnodaveDigitalChannel
    {
        private int pollingInterval;
        private readonly Action pollingAction;

        /// <summary>
        /// The polling interval in milliseconds
        /// </summary>
        public int PollingInterval
        {
            get => pollingInterval;
            set => pollingInterval = value;
        }

        /// <summary>
        /// The polling <see cref="Action"/>
        /// </summary>
        internal Action PollingAction => pollingAction;

        /// <summary>
        /// Create a new instance of <see cref="LibnodaveDigitalInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="memoryAddress">The memory address</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="pollingInterval">The polling interval (in milliseconds)</param>
        public LibnodaveDigitalInput(string code, int memoryAddress, IResource resource, int pollingInterval = 100) 
            : base(code, memoryAddress, resource)
        {
            this.pollingInterval = pollingInterval;

            pollingAction = async () =>
            {
                while (true)
                {
                    await (resource as LibnodaveResource).Receive(code);
                    await Task.Delay(pollingInterval);
                }
            };
        }

        /// <summary>
        /// Propagate the new value assigned to the
        /// <see cref="LibnodaveDigitalInput"/>
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="ValueChangedEventArgs"/></param>
        protected override async void PropagateValues(object sender, ValueChangedEventArgs e)
        {
            await (resource as LibnodaveResource).Receive(code);
            base.PropagateValues(sender, e);
        }
    }
}
