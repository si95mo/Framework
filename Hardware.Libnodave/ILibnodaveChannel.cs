using Core;

namespace Hardware.Libnodave
{
    /// <summary>
    /// Define the basic property of a channel for the <see cref="LibnodaveResource"/>
    /// </summary>
    public interface ILibnodaveChannel : IProperty
    {
        /// <summary>
        /// The <see cref="ILibnodaveChannel"/> memory address in the memory buffer
        /// </summary>
        int MemoryAddress { get; set; }

        /// <summary>
        /// The number of bytes relative to the variable in the memory buffer
        /// </summary>
        RepresentationBytes RepresentationBytes { get; set; }
    }
}