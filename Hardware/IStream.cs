using System.Text;

namespace Hardware
{
    /// <summary>
    /// Define a stream channel interface
    /// </summary>
    public interface IStream : IChannel<byte[]>
    {
        /// <summary>
        /// The <see cref="IStream"/> <see cref="System.Text.Encoding"/>
        /// </summary>
        Encoding Encoding { get; }

        /// <summary>
        /// The encoded value
        /// </summary>
        string EncodedValue { get; set; }
    }
}