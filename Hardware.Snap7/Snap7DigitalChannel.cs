using Core;

namespace Hardware.Snap7
{
    /// <summary>
    /// Define an abstract Snap7 channel for digital variables
    /// </summary>
    public abstract class Snap7DigitalChannel : Channel<bool>, ISnap7Channel
    {
        private NumericRepresentation numericRepresentation;
        private RepresentationBytes representationBytes;
        private int memoryAddress;
        private int dataBlock;

        protected IResource resource;

        public NumericRepresentation NumericRepresentation
        {
            get => numericRepresentation;
            set => _ = value;
        }

        public RepresentationBytes RepresentationBytes
        {
            get => representationBytes;
            set => _ = value;
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
        /// Initialize the <see cref="Snap7DigitalChannel"/> attributes
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="memoryAddress">The memory address</param>
        /// <param name="dataBlock">The data block number</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        protected Snap7DigitalChannel(string code, int memoryAddress, int dataBlock, IResource resource)
            : base(code)
        {
            this.memoryAddress = memoryAddress;
            this.dataBlock = dataBlock;

            numericRepresentation = NumericRepresentation.Boolean;
            representationBytes = RepresentationBytes.One;

            this.resource = resource;
            resource.Channels.Add(this);
        }
    }
}