using Core;
using System;
using System.Threading.Tasks;

namespace Hardware.Libnodave
{
    /// <summary>
    /// Implement an analog input for the <see cref="LibnodaveResource"/>
    /// </summary>
    public class LibnodaveAnalogInput : LibnodaveAnalogChannel
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
        /// Create a new instance of <see cref="LibnodaveAnalogInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="memoryAddress">The memory address</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="pollingInterval">The polling interval (in milliseconds)</param>
        /// <param name="bytes">The representation bytes</param>
        /// <param name="representation">The numeric representation</param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        public LibnodaveAnalogInput(string code, int memoryAddress, IResource resource, RepresentationBytes bytes,
            NumericRepresentation representation, int pollingInterval = 100, string measureUnit = "", string format = "0.000")
            : base(code, memoryAddress, resource, bytes, representation, measureUnit, format)
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
            pollingAction();
        }

        /// <summary>
        /// Propagate the new value assigned to the
        /// <see cref="LibnodaveAnalogInput"/>
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="ValueChangedEventArgs"/></param>
        protected override async void PropagateValues(object sender, ValueChangedEventArgs e)
        {
            await (resource as LibnodaveResource).Receive(Code);
            base.PropagateValues(sender, e);
        }
    }
}