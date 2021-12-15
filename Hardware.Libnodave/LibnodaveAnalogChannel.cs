using Core;

namespace Hardware.Libnodave
{
    /// <summary>
    /// Represent a generic analog channel for the <see cref="LibnodaveResource"/>
    /// </summary>
    public abstract class LibnodaveAnalogChannel : Channel<double>, ILibnodaveChannel
    {
        private int memoryAddress;
        private RepresentationBytes bytes;
        private NumericRepresentation representation;
        protected IResource resource;

        public int MemoryAddress { get => memoryAddress; set => memoryAddress = value; }

        public RepresentationBytes RepresentationBytes
        {
            get => bytes;
            set => bytes = value;
        }

        /// <summary>
        /// The <see cref="LibnodaveAnalogChannel"/> <see cref="NumericRepresentation"/>
        /// </summary>
        public NumericRepresentation Representation
        {
            get => representation;
            set => representation = value;
        }

        /// <summary>
        /// Create a new instance of <see cref="LibnodaveAnalogChannel"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="memoryAddress">The memory address</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="bytes">The <see cref="RepresentationBytes"/></param>
        /// <param name="representation">The <see cref="NumericRepresentation"/></param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        protected LibnodaveAnalogChannel(string code, int memoryAddress, IResource resource, RepresentationBytes bytes,
            NumericRepresentation representation, string measureUnit = "", string format = "0.000") : base(code, measureUnit, format, resource)
        {
            this.memoryAddress = memoryAddress;
            this.resource = resource;
            this.bytes = bytes;
            this.representation = representation;
        }
    }
}