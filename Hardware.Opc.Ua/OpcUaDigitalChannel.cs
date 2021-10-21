namespace Hardware.Opc.Ua
{
    /// <summary>
    /// Describe a generic Opc Ua digital channel
    /// </summary>
    public class OpcUaDigitalChannel : Channel<bool>, IOpcUaDigitalChannel
    {
        protected string namespaceConfiguration;
        protected IResource resource;

        /// <summary>
        /// The namespace configuration
        /// </summary>
        public string NamespaceConfiguration
        {
            get => namespaceConfiguration;
            set => namespaceConfiguration = value;
        }

        /// <summary>
        /// Create a new instance of <see cref="OpcUaAnalogChannel"/>
        /// </summary>
        /// <param name="code">THe code</param>
        /// <param name="namespaceConfiguration">The namespace configuration</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        protected OpcUaDigitalChannel(string code, string namespaceConfiguration, IResource resource) : base(code)
        {
            this.namespaceConfiguration = namespaceConfiguration;
            this.resource = resource;

            resource.Channels.Add(this);
        }
    }
}