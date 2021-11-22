using Core;
using System;
using System.Threading.Tasks;

namespace Hardware.Twincat
{
    public class TwincatAnalogInput : TwincatChannel<double>
    {
        private int pollingInterval;
        private readonly Action pollingAction;

        /// <summary>
        /// The polling interval (in milliseconds)
        /// </summary>
        public int PollingInterval 
        { 
            get => pollingInterval;
            set => pollingInterval = value; 
        }

        /// <summary>
        /// Create a new instance of <see cref="TwincatAnalogInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="variableName">The variable name in Twincat</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="pollingInterval">The polling interval (in milliseconds)</param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        public TwincatAnalogInput(string code, string variableName, IResource resource, int pollingInterval = 100,
            string measureUnit = "", string format = "0.000")
            : base(code, variableName, resource, measureUnit, format)
        {
            this.pollingInterval = pollingInterval;

            pollingAction = async () =>
            {
                while (true)
                {
                    await (resource as TwincatResource).Receive(code);
                    await Task.Delay(pollingInterval);
                }
            };
        }

        /// <summary>
        /// Propagate the new value assigned to the
        /// <see cref="TwincatAnalogInput"/>
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="ValueChangedEventArgs"/></param>
        protected override async void PropagateValues(object sender, ValueChangedEventArgs e)
        {
            await (resource as TwincatResource).Receive(code);
            base.PropagateValues(sender, e);
        }
    }
}
