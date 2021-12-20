using Core;
using System;
using System.Threading.Tasks;

namespace Hardware.Snap7
{
    /// <summary>
    /// Implement a Snap7 digital input channel
    /// </summary>
    public class Snap7DigitalInput : Snap7DigitalChannel
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
        /// Create a new instance of <see cref="Snap7DigitalInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="memoryAddress">The memory address</param>
        /// <param name="dataBlock">The data block number</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="pollingInterval">The polling interval (in milliseconds)</param>
        public Snap7DigitalInput(string code, int memoryAddress, int dataBlock, IResource resource, int pollingInterval = 100)
            : base(code, memoryAddress, dataBlock, resource)
        {
            this.pollingInterval = pollingInterval;

            pollingAction = async () =>
            {
                while (true)
                {
                    await (resource as Snap7Resource).Receive(code);
                    await Task.Delay(pollingInterval);
                }
            };
            pollingAction();
        }

        /// <summary>
        /// Propagate the new value assigned to the
        /// <see cref="Snap7DigitalInput"/>
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="ValueChangedEventArgs"/></param>
        protected override async void PropagateValues(object sender, ValueChangedEventArgs e)
        {
            await (resource as Snap7Resource).Receive(code);
            base.PropagateValues(sender, e);
        }
    }
}
