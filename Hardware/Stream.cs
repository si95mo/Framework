using System.Text;

namespace Hardware
{
    /// <summary>
    /// Implement an <see cref="IStream"/>
    /// </summary>
    public class Stream : Channel<byte[]>, IStream
    {
        public Encoding Encoding { get; protected set; }

        public string EncodedValue 
        { 
            get => Encoding.GetString(Value);
            set => Value = Encoding.GetBytes(value); 
        }

        /// <summary>
        /// Create a new instance of <see cref="Stream"/>
        /// </summary>
        /// <param name="code"></param>
        /// <param name="encoding">The <see cref="System.Text.Encoding"/></param>
        public Stream(string code, Encoding encoding) : base(code)
        {
            Encoding = encoding;
        }
    }
}
