using Core;

namespace Hardware.Opc.Ua
{
    /// <summary>
    /// Describe a basic Opc Ua analog channel prototype. See <see cref="IOpcUaChannel"/>
    /// </summary>
    public interface IOpcUaAnalogChannel : IProperty<double>, IOpcUaChannel
    { }

    /// <summary>
    /// Describe a basic Opc Ua digital channel prototype. See <see cref="IOpcUaChannel"/>
    /// </summary>
    public interface IOpcUaDigitalChannel : IProperty<bool>, IOpcUaChannel
    { }

    /// <summary>
    /// Describe a basic Opc Ua channel prototype
    /// </summary>
    public interface IOpcUaChannel : IProperty
    {
        /// <summary>
        /// The <see cref="IOpcUaAnalogChannel"/> namespace configuration
        /// (e.g. ns=2;s=Temperature)
        /// </summary>
        string NamespaceConfiguration { get; set; }
    }
}