namespace Hardware.Libnodave
{
    /// <summary>
    /// Implememt a digital output channel for the <see cref="LibnodaveResource"/>
    /// </summary>
    public class LibnodaveDigitalOutput : LibnodaveDigitalChannel
    {
        /// <summary>
        /// Create a new instance of <see cref="LibnodaveDigitalOutput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="memoryAddress">The memory address</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        public LibnodaveDigitalOutput(string code, int memoryAddress, IResource resource) : base(code, memoryAddress, resource)
        {
            ValueChanged += LibnodaveDigitalOutput_ValueChanged;
        }

        private async void LibnodaveDigitalOutput_ValueChanged(object sender, Core.ValueChangedEventArgs e)
        {
            await (resource as LibnodaveResource).Send(code);
        }
    }
}