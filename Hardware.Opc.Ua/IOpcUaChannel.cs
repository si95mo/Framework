using Core;

namespace Hardware.Opc.Ua
{
    public interface IOpcUaChannel : IProperty<double>
    {
        /// <summary>
        /// The <see cref="IOpcUaChannel"/> namespace configuration
        /// (e.g. ns=2;s=Temperature)
        /// </summary>
        string NamespaceConfiguration { get; set; }
    }
}