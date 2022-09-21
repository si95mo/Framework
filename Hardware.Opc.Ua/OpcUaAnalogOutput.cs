namespace Hardware.Opc.Ua
{
    public class OpcUaAnalogOutput : OpcUaAnalogChannel
    {
        /// <summary>
        /// Create a new instance of <see cref="OpcUaAnalogOutput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="namespaceConfiguration">The namespace configuration</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        public OpcUaAnalogOutput(string code, string namespaceConfiguration, IResource resource, string measureUnit = "",
            string format = "0.000") : base(code, namespaceConfiguration, resource, measureUnit, format)
        {
            ValueChanged += OpcUaOutput_ValueChanged;
        }

        private async void OpcUaOutput_ValueChanged(object sender, Core.ValueChangedEventArgs e)
        {
            await (resource as OpcUaResource).Send(Code);
        }
    }
}