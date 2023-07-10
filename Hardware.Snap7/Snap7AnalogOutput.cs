using Core;

namespace Hardware.Snap7
{
    /// <summary>
    /// Implement a Snap7 analog output channel
    /// </summary>
    public class Snap7AnalogOutput : Snap7AnalogChannel
    {
        /// <summary>
        /// Create a new instance of <see cref="Snap7AnalogOutput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="memoryAddress">The memory address</param>
        /// <param name="dataBlock">The data block</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="representationBytes">The <see cref="RepresentationBytes"/></param>
        /// <param name="numericRepresentation">The <see cref="NumericRepresentation"/></param>
        /// <param name="reverse">The reverse option</param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        public Snap7AnalogOutput(string code, int memoryAddress, int dataBlock, IResource resource, RepresentationBytes representationBytes,
            NumericRepresentation numericRepresentation, bool reverse = false, string measureUnit = "", string format = "0.000")
            : base(code, memoryAddress, dataBlock, resource, representationBytes, numericRepresentation, reverse, measureUnit, format)
        {
            ChannelType = ChannelType.AnalogOutput;
            ValueChanged += Snap7AnalogOutput_ValueChanged;
        }

        private void Snap7AnalogOutput_ValueChanged(object sender, ValueChangedEventArgs e)
            => (resource as Snap7Resource).Send(Code);
    }
}