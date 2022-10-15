namespace Hardware.Twincat
{
    /// <summary>
    /// Implement a Twincat multi-sample analog input
    /// </summary>
    public class TwincatMultiSampleAnalogInput : TwincatChannel<double[]>
    {
        /// <summary>
        /// Create a new instance of <see cref="TwincatMultiSampleAnalogInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="variableName">The variable name in Twincat</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        protected TwincatMultiSampleAnalogInput(string code, string variableName, IResource resource, string measureUnit = "", string format = "0.000")
            : base(code, variableName, resource, measureUnit, format)
        { }

        public override void Attach()
        {
            throw new System.NotImplementedException();
        }
    }
}