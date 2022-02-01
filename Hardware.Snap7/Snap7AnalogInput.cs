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
        /// <param name="reverse">The reverse option</param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        public Snap7AnalogInput(string code, int memoryAddress, int dataBlock, IResource resource, RepresentationBytes representationBytes,
            NumericRepresentation numericRepresentation, int pollingInterval = 100, bool reverse = false, string measureUnit = "", string format = "0.000")
            : base(code, memoryAddress, dataBlock, resource, representationBytes, numericRepresentation, reverse, measureUnit, format)
        {
            this.pollingInterval = pollingInterval;

            pollingAction = async () =>
            {
                while (true)
                {
                    if (resource.Status.Value == ResourceStatus.Executing)
                        AcquireValue();

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
        protected override void PropagateValues(object sender, ValueChangedEventArgs e)
        {
            AcquireValue();
            base.PropagateValues(sender, e);
        }

        /// <summary>
        /// Acquire the <see cref="Snap7AnalogInput"/> associated value
        /// from the <see cref="Snap7Resource"/>
        /// </summary>
        private void AcquireValue()
        {
            if (resource.Status.Value == ResourceStatus.Executing)
            {
                // Memory buffer manipulation - get only the elements of interest
                int n = ExtractNumberOfBytes();
                byte[] array = new byte[n];
                byte[] buffer = (resource as Snap7Resource).GetDataBlockBuffer(DataBlock);
                Array.Copy(buffer, MemoryAddress, array, 0, n);

                if (Reverse)
                    Array.Reverse(array);

                // Channel value assignment
                switch (RepresentationBytes)
                {
                    case RepresentationBytes.Eight:
                        Value = BitConverter.ToDouble(array, 0);
                        break;

                    case RepresentationBytes.Four:
                        switch (NumericRepresentation)
                        {
                            case NumericRepresentation.Single:
                                Value = BitConverter.ToSingle(array, 0);
                                break;

                            case NumericRepresentation.Int32:
                                Value = BitConverter.ToUInt32(array, 0);
                                break;
                        }
                        break;

                    case RepresentationBytes.Two:
                        switch (NumericRepresentation)
                        {
                            case NumericRepresentation.UInt16:
                                Value = BitConverter.ToUInt16(array, 0);
                                break;

                            case NumericRepresentation.Int16:
                                Value = BitConverter.ToInt16(array, 0);
                                break;
                        }
                        break;

                    case RepresentationBytes.One:
                        Value = array[0];
                        break;
                }
            }
        }
    }
}