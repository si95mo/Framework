namespace Hardware.Twincat
{
    public class TwincatDigitalOutput : TwincatChannel<bool>, ITwincatChannel
    {
        /// <summary>
        /// Create a new instance of <see cref="TwincatDigitalOutput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="variableName">The variable name in Twincat</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        public TwincatDigitalOutput(string code, string variableName, IResource resource) 
            : base(code, variableName, resource, measureUnit: "", format: "0")
        {
            ValueChanged += TwincatAnalogOutput_ValueChanged;
        }

        private async void TwincatAnalogOutput_ValueChanged(object sender, Core.ValueChangedEventArgs e)
        {
            await (resource as TwincatResource).Send(code);
        }
    }
}
