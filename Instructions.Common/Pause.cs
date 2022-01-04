using Core.Parameters;
using System;
using System.Threading.Tasks;

namespace Instructions.Common
{
    /// <summary>
    /// Implement an <see cref="Instruction"/> that waits for an amount of time
    /// </summary>
    public class Pause : Instruction
    {
        private TimeSpanParameter time;

        /// <summary>
        /// Create a new instance of <see cref="Pause"/>
        /// </summary>
        /// <param name="time">The time to wait</param>
        public Pause(TimeSpan time) : base("Wait")
        {
            this.time = new TimeSpanParameter($"{Code}.Time");
            this.time.Value = time;

            InputParameters.Add(this.time);
        }

        /// <summary>
        /// Create a new instance of <see cref="Pause"/>
        /// </summary>
        /// <param name="time">The time to wait (in milliseconds)</param>
        public Pause(double time) : this(TimeSpan.FromMilliseconds(time))
        { }

        public override async Task ExecuteInstruction()
            => await Task.Delay(time.Value);
    }
}