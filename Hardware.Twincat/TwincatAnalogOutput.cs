namespace Hardware.Twincat
{
    /// <summary>
    /// Implement a Twincat analog output
    /// </summary>
    public class TwincatAnalogOutput : TwincatChannel<double>, ITwincatChannel
    {
        /// <summary>
        /// Create a new instance of <see cref="TwincatAnalogOutput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="variableName">The variable name in Twincat</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        public TwincatAnalogOutput(string code, string variableName, IResource resource, string measureUnit = "",
            string format = "0.000") : base(code, variableName, resource, measureUnit, format)
        {
            ValueChanged += TwincatAnalogOutput_ValueChanged;
        }

        private void TwincatAnalogOutput_ValueChanged(object sender, Core.ValueChangedEventArgs e)
            => (resource as TwincatResource).Send(code);
    }
}