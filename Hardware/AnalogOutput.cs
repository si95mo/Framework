using System;

namespace Hardware
{
    /// <summary>
    /// Implement an analog input channel.
    /// See also <see cref="Channel{T}"/> and <see cref="IChannel{T}"/>
    /// </summary>
    public class AnalogOutput : Channel<double>, IAnalogOutput
    {
        /// <summary>
        /// Create a new instance of <see cref="AnalogOutput"/>
        /// </summary>
        public AnalogOutput() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Create a new instance of <see cref="AnalogOutput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="format">The format</param>
        /// <param name="measureUnit">The measure unit</param>
        public AnalogOutput(string code, string measureUnit = "", string format = "0.0") : base(code, measureUnit, format)
        { }
    }
}