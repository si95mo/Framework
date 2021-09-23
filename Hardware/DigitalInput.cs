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
        /// The <see cref="AnalogInput"/> value;
        /// </summary>
        public override bool Value => value;

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
        { }
    }
}