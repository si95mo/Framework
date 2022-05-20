namespace Hardware.Twincat
{
    /// <summary>
    /// Implement a Twincat digital input
    /// </summary>
    public class TwincatDigitalInput : TwincatChannel<bool>, ITwincatChannel
    {
        /// <summary>
        /// Create a new instance of <see cref="TwincatDigitalInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="variableName">The variable name</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        public TwincatDigitalInput(string code, string variableName, IResource resource)
            : base(code, variableName, resource, measureUnit: "", format: "0")
        { }
    }
}