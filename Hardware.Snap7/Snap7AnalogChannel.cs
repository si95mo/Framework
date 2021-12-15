using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// Initialize the <see cref="Snap7AnalogChannel"/> attributes
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="memoryAddress">The memory address</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        protected Snap7AnalogChannel(string code, int memoryAddress, IResource resource, RepresentationBytes representationBytes, 
            NumericRepresentation numericRepresentation, string measureUnit = "", string format = "0.000") : base(code, measureUnit, format, resource)
        {
            this.memoryAddress = memoryAddress;

            this.numericRepresentation = numericRepresentation;
            this.representationBytes = representationBytes;
        }
    }
}
