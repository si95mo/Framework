namespace Hardware.Twincat
{
    /// <summary>
    /// Implement a twincat analog input
    /// </summary>
    public class TwincatAnalogInput : TwincatChannel<double>
    {
        /// <summary>
        /// Create a new instance of <see cref="TwincatAnalogInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="variableName">The variable name in Twincat</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        public TwincatAnalogInput(string code, string variableName, IResource resource, string measureUnit = "", string format = "0.000")
            : base(code, variableName, resource, measureUnit, format)
        { }
    }
}