using Core;
using System.Text;

namespace Hardware
{
    /// <summary>
    /// Define an input <see cref="Stream"/>
    /// </summary>
    public class StreamInput : Stream, IReadOnlyProperty<byte[]>
    {
        /// <summary>
        /// Create a new instance of <see cref="StreamInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="encoding">The <see cref="Encoding"/></param>
        public StreamInput(string code, Encoding encoding) : base(code, encoding)
        { }

        /// <summary>
        /// Create a new instance of <see cref="StreamInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="encoding">The <see cref="Encoding"/></param>
        /// <param name="resource">The <see cref="IResource"/></param>
        public StreamInput(string code, Encoding encoding, IResource resource) : base(code, encoding, resource)
        { }
    }
}