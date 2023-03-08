using System;

namespace Hardware
{
    /// <summary>
    /// Implement a digital input channel.
    /// See also <see cref="Channel{T}"/> and <see cref="IChannel{T}"/>
    /// </summary>
    public class DigitalInput : Channel<bool>, IDigitalInput
    {
        /// <summary>
        /// Create a new instance of <see cref="DigitalInput"/>
        /// </summary>
        public DigitalInput() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Create a new instance of <see cref="DigitalInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        public DigitalInput(string code) : base(code)
        {
            ChannelType = ChannelType.DigitalInput;
        }

        /// <summary>
        /// Create a new instance of <see cref="DigitalInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        public DigitalInput(string code, IResource resource) : base(code, string.Empty, string.Empty, resource)
        { }
    }
}