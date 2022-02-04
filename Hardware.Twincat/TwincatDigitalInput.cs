using Core;
using System;
using System.Threading.Tasks;

namespace Hardware.Twincat
{
    public class TwincatDigitalInput : TwincatChannel<bool>, ITwincatChannel
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
        /// Create a new instance of <see cref="TwincatDigitalInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="variableName">The variable name</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="pollingInterval">The polling interval (in milliseconds)</param>
        protected TwincatDigitalInput(string code, string variableName, IResource resource, int pollingInterval = 100)
            : base(code, variableName, resource, measureUnit: "", format: "0")
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