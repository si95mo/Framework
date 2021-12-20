namespace Hardware.Snap7
{
    /// <summary>
    /// Implement a Snap7 digital output channel
    /// </summary>
    public class Snap7DigitalOutput : Snap7DigitalChannel
    {
        /// <summary>
        /// Create a new instance of <see cref="Snap7DigitalOutput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="memoryAddress">The memory address</param>
        /// <param name="dataBlock">The data block number</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        public Snap7DigitalOutput(string code, int memoryAddress, int dataBlock, IResource resource) 
            : base(code, memoryAddress, dataBlock, resource)
        {
            ValueChanged += Snap7DigitalOutput_ValueChanged;
        }

        private async void Snap7DigitalOutput_ValueChanged(object sender, Core.ValueChangedEventArgs e)
            => await (resource as Snap7Resource).Send(Code);
    }
}
