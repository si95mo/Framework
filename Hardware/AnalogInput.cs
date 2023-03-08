using System;

namespace Hardware
{
    /// <summary>
    /// Implement an analog input channel.
    /// See also <see cref="Channel{T}"/> and <see cref="IChannel{T}"/>
    /// </summary>
    public class AnalogInput : Channel<double>, IAnalogInput
    {
        /// <summary>
        /// Create a new instance of <see cref="AnalogInput"/>
        /// </summary>
        public AnalogInput() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Create a new instance of <see cref="AnalogInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="format">The format</param>
        /// <param name="measureUnit">The measure unit</param>
        public AnalogInput(string code, string measureUnit = "", string format = "0.0") : base(code, measureUnit, format)
        {
            ChannelType = ChannelType.AnalogInput;
        }
    }
}