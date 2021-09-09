namespace Hardware.Opc.Ua
{
    public class OpcUaOutput : OpcUaChannel
    {
        /// <summary>
        /// Create a new instance of <see cref="OpcUaOutput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="namespaceConfiguration">The namespace configuration</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        public OpcUaOutput(string code, string namespaceConfiguration, IResource resource, string measureUnit = "",
            string format = "0.000") : base(code, namespaceConfiguration, resource)
        {
            this.measureUnit = measureUnit;
            this.format = format;

            ValueChanged += OpcUaOutput_ValueChanged;
        }

        private void OpcUaOutput_ValueChanged(object sender, Core.ValueChangedEventArgs e)
        {
            (resource as OpcUaResource).Send(code);
        }
    }
}