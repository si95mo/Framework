using System;
using System.Threading.Tasks;

namespace Core.Parameters
{
    /// <summary>
    /// Implement a <see cref="Parameter{T}"/> that represent a trigger
    /// </summary>
    public class TriggerParameter : Parameter<bool>, IBoolParameter
    {
        private TimeSpan tOn;
        private TimeSpan preTriggerTime;
        private bool risingTrigger;

        /// <summary>
        /// Create a new instance of <see cref="TriggerParameter"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="value">The initial value</param>
        /// <param name="tOn">The on time of the trigger (as a <see cref="TimeSpan"/></param>
        public TriggerParameter(string code, bool value, TimeSpan tOn) : base(code)
        {
            this.tOn = tOn;
            preTriggerTime = TimeSpan.Zero;
            risingTrigger = !value; // If value is false, the trigger is a rising one, otherwise falling

            Value = value;
        }

        public TriggerParameter(string code, bool value, TimeSpan tOn, TimeSpan preTriggerTime) : this(code, value, tOn)
        {
            this.preTriggerTime = preTriggerTime;
        }

        /// <summary>
        /// Create a new instance of <see cref="TriggerParameter"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="value">The initial value</param>
        /// <param name="tOn">The on time of the trigger (in milliseconds)</param>
        public TriggerParameter(string code, bool value, int tOn) : this(code, value, TimeSpan.FromMilliseconds(tOn))
        { }

        /// <summary>
        /// Create a new instance of <see cref="TriggerParameter"/> with a rising edge and an on time of 100ms
        /// </summary>
        public TriggerParameter() : this(Guid.NewGuid().ToString(), false, TimeSpan.FromMilliseconds(100))
        { }

        /// <summary>
        /// Start the trigger
        /// </summary>
        /// <returns>The (async) <see cref="Task"/></returns>
        public async Task StartTrigger()
        {
            Value = !risingTrigger; // If the trigger is a rising one, the initial value must be false

            // Wait for the pre trigger time, if not equal to 0
            if (preTriggerTime != TimeSpan.Zero)
                await Task.Delay(preTriggerTime);

            Value = !value; // Toggle the trigger value
            await Task.Delay(tOn); // And wait the on time

            Value = !value; // Reset the trigger value;
        }

        /// <summary>
        /// Stop the actual trigger generation
        /// </summary>
        public void StopTrigger()
            => Value = risingTrigger; // If the trigger is a rising one, the default value must be true (or false otherwise)
    }
}