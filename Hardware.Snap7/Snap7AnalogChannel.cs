using Core;

namespace Hardware.Snap7
{
    /// <summary>
    /// Define an abstract Snap7 channel for analog variables
    /// </summary>
    public abstract class Snap7AnalogChannel : Channel<double>, ISnap7Channel
    {
        private NumericRepresentation numericRepresentation;
        private RepresentationBytes representationBytes;
        private int memoryAddress;
        private int dataBlock;

        protected IResource resource;

        public NumericRepresentation NumericRepresentation
        {
            get => numericRepresentation;
            set => numericRepresentation = value;
        }

        public RepresentationBytes RepresentationBytes
        {
            get => representationBytes;
            set => representationBytes = value;
        }

        public int MemoryAddress
        {
            get => memoryAddress;
            set => memoryAddress = value;
        }

        public int DataBlock
        {
            get => dataBlock;
            set => dataBlock = value;
        }

        /// <summary>
        /// Initialize the <see cref="Snap7AnalogChannel"/> attributes
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="memoryAddress">The memory address</param>
        /// <param name="dataBlock">The data block number</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="representationBytes">The <see cref="Core.RepresentationBytes"/></param>
        /// <param name="numericRepresentation">The <see cref="Core.NumericRepresentation"/></param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        protected Snap7AnalogChannel(string code, int memoryAddress, int dataBlock, IResource resource, RepresentationBytes representationBytes,
            NumericRepresentation numericRepresentation, string measureUnit = "", string format = "0.000") : base(code, measureUnit, format, resource)
        {
            this.memoryAddress = memoryAddress;
            this.dataBlock = dataBlock;

            this.numericRepresentation = numericRepresentation;
            this.representationBytes = representationBytes;

            this.resource = resource;
            resource.Channels.Add(this);
        }
    }
}