using System.Text;

namespace Hardware
{
    /// <summary>
    /// Define a <see cref="Resource"/> with a input and an output <see cref="Stream"/>
    /// </summary>
    public abstract class StreamResource : Resource
    {
        /// <summary>
        /// The input <see cref="Stream"/>
        /// </summary>
        public Stream Input { get; protected set; }

        /// <summary>
        /// The output <see cref="Stream"/>
        /// </summary>
        public Stream Output { get; protected set; }

        /// <summary>
        /// The terminator sequence in the stream
        /// </summary>
        public byte[] TerminatorSequence {get => FrameDetector.TerminatorSequence; protected set => FrameDetector.TerminatorSequence = value; }

        /// <summary>
        /// The <see cref="FrameDetector"/>
        /// </summary>
        public FrameDetector FrameDetector { get; protected set; }

        /// <summary>
        /// Create a new instance of <see cref="StreamResource"/>
        /// </summary>
        /// <param name="code"></param>
        protected StreamResource(string code) : base(code)
        { }

        /// <summary>
        /// Create a new instance of <see cref="StreamResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="encoding">The <see cref="Encoding"/></param>
        /// <param name="terminatorSequence">The terminator sequence in the stream</param>
        protected StreamResource(string code, Encoding encoding, string terminatorSequence) : base(code)
        {
            Input = new Stream($"{Code}.{nameof(Input)}", encoding);
            Output = new Stream($"{Code}.{nameof(Output)}", encoding);

            FrameDetector = new FrameDetector(encoding.GetBytes(terminatorSequence));

            Channels.Add(Input);
            Channels.Add(Output);
        }
    }
}