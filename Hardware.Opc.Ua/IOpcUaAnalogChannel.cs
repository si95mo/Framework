using Core;

namespace Hardware.Opc.Ua
{
    public interface IOpcUaAnalogChannel : IProperty<double>, IOpcUaChannel
    { }

    public interface IOpcUaChannel : IProperty
    {
        /// <summary>
        /// The <see cref="IOpcUaAnalogChannel"/> namespace configuration
        /// (e.g. ns=2;s=Temperature)
        /// </summary>
        string NamespaceConfiguration { get; set; }
    }
}