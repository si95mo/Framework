using System.Text;

namespace Core.Parameters
{
    public class StreamParameter : Parameter<byte[]>, IStreamParameter
    {
        public Encoding Encoding { get; protected set; }

        public string EncodedValue
        {
            get => Encoding.GetString(Value);
            set => Value = Encoding.GetBytes(value);
        }

        /// <summary>
        /// Create a new instance of <see cref="StreamParameter"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="encoding">The <see cref="Encoding"/></param>
        public StreamParameter(string code, Encoding encoding) : base(code)
        {
            Encoding = encoding;
        }
    }
}
