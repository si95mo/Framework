using System;

namespace Hardware
{
    /// <summary>
    /// Implement a digital output channel.
    /// See also <see cref="Channel{T}"/> and <see cref="IChannel{T}"/>
    /// </summary>
    public class DigitalOutput : Channel<bool>, IDigitalOutput
    {
        /// <summary>
        /// Create a new instance of <see cref="DigitalOutput"/>
        /// </summary>
        public DigitalOutput() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Create a new instance of <see cref="DigitalOutput"/>
        /// </summary>
        /// <param name="code">The code</param>
        public DigitalOutput(string code) : base(code)
        {
            ChannelType = ChannelType.DigitalOutput;
        }

        /// <summary>
        /// Create a new instance of <see cref="DigitalOutput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        public DigitalOutput(string code, IResource resource) : base(code, string.Empty, string.Empty, resource)
        { }
    }
}