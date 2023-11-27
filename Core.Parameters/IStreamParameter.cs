using System.Text;

namespace Core.Parameters
{
    public interface IStreamParameter : IParameter<byte[]>
    {
        /// <summary>
        /// The <see cref="IStreamParameter"/> <see cref="System.Text.Encoding"/>
        /// </summary>
        Encoding Encoding { get; }

        /// <summary>
        /// The encoded value
        /// </summary>
        string EncodedValue { get; set; }
    }
}