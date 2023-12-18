namespace Hardware
{
    /// <summary>
    /// Define an interface to detect frames in a stream
    /// </summary>
    public interface IFrameDetector
    {
        /// <summary>
        /// The terminator sequence of the frame
        /// </summary>
        byte[] TerminatorSequence { get; set; }

        /// <summary>
        /// Try to get the last retrieved frame data
        /// </summary>
        /// <param name="data">The data retrieved</param>
        /// <returns>The operation result</returns>
        bool TryGet(out byte[] data);

        /// <summary>
        /// Add new bytes to the frame
        /// </summary>
        /// <param name="data">The data to add</param>
        void Add(byte[] data);

        /// <summary>
        /// Clear the <see cref="IFrameDetector"/>
        /// </summary>
        void Clear();
    }
}
