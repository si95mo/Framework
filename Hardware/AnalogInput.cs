namespace Hardware
{
    /// <summary>
    /// Implement an analog input channel.
    /// See also <see cref="Channel{T}"/> and <see cref="IChannel{T}"/>
    /// </summary>
    public class AnalogInput : Channel<double>
    {
        private string measureUnit;
        private string format;

        /// <summary>
        /// The <see cref="AnalogInput"/> value;
        /// </summary>
        public override double Value => value;

        /// <summary>
        /// Create a new instance of <see cref="AnalogInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="format">The format</param>
        /// <param name="measureUnit">The measure unit</param>
        public AnalogInput(string code, string measureUnit = "", string format = "0.0") : base(code)
        {
            this.measureUnit = measureUnit;
            this.format = format;
        }

        /// <summary>
        /// Return a description of the object
        /// See also <see cref="object.ToString()"/>
        /// </summary>
        /// <returns>The description of the object</returns>
        public override string ToString()
        {
            string description = $"{value.ToString(format)}{measureUnit}";

            return description;
        }
    }
}
