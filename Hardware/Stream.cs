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
            Initialize(encoding);
        }

        /// <summary>
        /// Create a new instance of <see cref="Stream"/>
        /// </summary>
        /// <param name="code"></param>
        /// <param name="encoding">The <see cref="System.Text.Encoding"/></param>
        /// <param name="resource">The <see cref="IResource"/></param>
        public Stream(string code, Encoding encoding, IResource resource) :base(code, string.Empty, string.Empty, resource)
        {
            Initialize(encoding);
        }

        /// <summary>
        /// Initialize the attributes
        /// </summary>
        /// <param name="encoding">The <see cref="System.Text.Encoding"/></param>
        private void Initialize(Encoding encoding)
        {
            Encoding = encoding;
            ChannelType = ChannelType.Stream;

            Value = new byte[] { 0b00000000 };
        }

        public override string ToString()
            => EncodedValue;
    }
}