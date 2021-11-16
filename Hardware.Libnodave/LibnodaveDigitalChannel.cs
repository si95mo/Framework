using Core;

namespace Hardware.Libnodave
{
    /// <summary>
    /// Represent an abstract digital channel for the <see cref="LibnodaveResource"/>
    /// </summary>
    public abstract class LibnodaveDigitalChannel : Channel<bool>, ILibnodaveChannel
    {
        private int memoryAddress;
        protected IResource resource;

        public int MemoryAddress { get => memoryAddress; set => memoryAddress = value; }
        public RepresentationBytes RepresentationBytes 
        { 
            get => RepresentationBytes.One; 
            set => _ = value; 
        }

        /// <summary>
        /// Create a new instance <see cref="LibnodaveDigitalChannel"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="memoryAddress">The memory address</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        protected LibnodaveDigitalChannel(string code, int memoryAddress, IResource resource) : base(code)
        {
            this.memoryAddress = memoryAddress;
            this.resource = resource;

            resource.Channels.Add(this);
        }
    }
}
