using Core;

namespace Hardware.Snap7
{
    /// <summary>
    /// Describe a basic prototype for a channel that communicate with a Siemens PLC
    /// using the <see cref="Snap7Resource"/>
    /// </summary>
    public interface ISnap7Channel : IProperty
    {
        /// <summary>
        /// The <see cref="ISnap7Channel"/> <see cref="Core.NumericRepresentation"/>
        /// </summary>
        NumericRepresentation NumericRepresentation { get; set; }

        /// <summary>
        /// The <see cref="ISnap7Channel"/> <see cref="Core.RepresentationBytes"/>
        /// </summary>
        RepresentationBytes RepresentationBytes { get; set; }

        /// <summary>
        /// The <see cref="ISnap7Channel"/> memory address (starting from 0)
        /// </summary>
        int MemoryAddress { get; set; }
    }
}