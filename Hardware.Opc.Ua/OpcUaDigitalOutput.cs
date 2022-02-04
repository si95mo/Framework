namespace Hardware.Opc.Ua
{
    /// <summary>
    /// Implement an Opc Ua digital output
    /// </summary>
    public class OpcUaDigitalOutput : OpcUaDigitalChannel
    {
        /// <summary>
        /// Create a new instance of <see cref="OpcUaDigitalOutput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="namespaceConfiguration">The namespace configuration</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        public OpcUaDigitalOutput(string code, string namespaceConfiguration, IResource resource) : base(code, namespaceConfiguration, resource)
        {
            ValueChanged += OpcUaOutput_ValueChanged;
        }

        private async void OpcUaOutput_ValueChanged(object sender, Core.ValueChangedEventArgs e)
        {
            await (resource as OpcUaResource).Send(code);
        }
    }
}