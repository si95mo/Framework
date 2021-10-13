namespace Hardware.Opc.Ua
{
    public class OpcUaChannel : Channel<double>, IOpcUaChannel
    {
        protected string namespaceConfiguration;
        protected IResource resource;

        public string NamespaceConfiguration
        {
            get => namespaceConfiguration;
            set => namespaceConfiguration = value;
        }

        /// <summary>
        /// Create a new instance of <see cref="OpcUaChannel"/>
        /// </summary>
        /// <param name="code">THe code</param>
        /// <param name="namespaceConfiguration">The namespace configuration</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        protected OpcUaChannel(string code, string namespaceConfiguration, IResource resource) : base(code)
        {
            this.namespaceConfiguration = namespaceConfiguration;
            this.resource = resource;

            resource.Channels.Add(this);
        }

        public override string ToString()
        {
            string description = $"{value.ToString(format)}{measureUnit}";

            return description;
        }
    }
}