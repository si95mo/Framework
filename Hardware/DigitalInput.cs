using System;

namespace Hardware
{
    /// <summary>
    /// Implement a digital input channel.
    /// See also <see cref="Channel{T}"/> and <see cref="IChannel{T}"/>
    /// </summary>
    public class DigitalInput : Channel<bool>
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

        /// <summary>
        /// Return a description of the object
        /// See also <see cref="object.ToString()"/>
        /// </summary>
        /// <returns>The description of the object</returns>
        public override string ToString()
        {
            string description = $"{value}";

            return description;
        }
    }
}
