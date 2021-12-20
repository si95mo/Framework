using Core;
using System;
using System.Threading.Tasks;

namespace Hardware.Snap7
{
    /// <summary>
    /// Implement a Snap7 analog input channel
    /// </summary>
    public class Snap7AnalogInput : Snap7AnalogChannel
    {
        private int pollingInterval;
        private readonly Action pollingAction;

        /// <summary>
        /// The polling interval (in milliseconds)
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
        /// Create a new instance of <see cref="Snap7AnalogInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="memoryAddress">The memory address</param>
        /// <param name="dataBlock">The data block number</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="representationBytes">The <see cref="RepresentationBytes"/></param>
        /// <param name="numericRepresentation">The <see cref="NumericRepresentation"/></param>
        /// <param name="pollingInterval">The polling interval (in  milliseconds)</param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        public Snap7AnalogInput(string code, int memoryAddress, int dataBlock, IResource resource, RepresentationBytes representationBytes,
            NumericRepresentation numericRepresentation, int pollingInterval = 100, string measureUnit = "", string format = "0.000")
            : base(code, memoryAddress, dataBlock, resource, representationBytes, numericRepresentation, measureUnit, format)
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
        /// <see cref="Snap7AnalogInput"/>
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
