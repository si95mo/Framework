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
        private bool reverse;

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
        /// The reverse option
        /// </summary>
        internal bool Reverse => reverse;

        /// <summary>
        /// Initialize the <see cref="Snap7AnalogChannel"/> attributes
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="memoryAddress">The memory address</param>
        /// <param name="dataBlock">The data block number</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="representationBytes">The <see cref="Core.RepresentationBytes"/></param>
        /// <param name="numericRepresentation">The <see cref="Core.NumericRepresentation"/></param>
        /// <param name="reverse">The reverse array option</param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        protected Snap7AnalogChannel(string code, int memoryAddress, int dataBlock, IResource resource, RepresentationBytes representationBytes,
            NumericRepresentation numericRepresentation, bool reverse = false, string measureUnit = "", string format = "0.000")
            : base(code, measureUnit, format, resource)
        {
            this.memoryAddress = memoryAddress;
            this.dataBlock = dataBlock;

            this.numericRepresentation = numericRepresentation;
            this.representationBytes = representationBytes;

            this.reverse = reverse;

            this.resource = resource;
            resource.Channels.Add(this);
        }

        /// <summary>
        /// Extract the number of bytes based on <see cref="RepresentationBytes"/>
        /// </summary>
        /// <returns>The number of bytes</returns>
        protected int ExtractNumberOfBytes()
        {
            int numberOfBytes;

            switch (RepresentationBytes)
            {
                case RepresentationBytes.One: // byte
                    numberOfBytes = 1;
                    break;

                case RepresentationBytes.Two: // ushort
                    numberOfBytes = 2;
                    break;

                case RepresentationBytes.Four: // int and float
                    numberOfBytes = 4;
                    break;

                case RepresentationBytes.Eight: // double
                    numberOfBytes = 8;
                    break;

                default:
                    numberOfBytes = 4;
                    break;
            }

            return numberOfBytes;
        }
    }
}