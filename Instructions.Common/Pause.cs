using Core.Parameters;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Instructions.Common
{
    /// <summary>
    /// Implement an <see cref="Instruction"/> that waits for an amount of time
    /// </summary>
    public class Pause : Instruction
    {
        private TimeSpanParameter time;
        private TimeSpanParameter ElapsedTime;

        /// <summary>
        /// Create a new instance of <see cref="Pause"/>
        /// </summary>
        /// <param name="time">The time to wait</param>
        public Pause(TimeSpan time) : base("Wait")
        {
            this.time = new TimeSpanParameter($"{Code}.Time");
            this.time.Value = time;

            ElapsedTime = new TimeSpanParameter($"{Code}.{nameof(ElapsedTime)}");

            InputParameters.Add(this.time);
            OutputParameters.Add(ElapsedTime);
        }

        /// <summary>
        /// Create a new instance of <see cref="Pause"/>
        /// </summary>
        /// <param name="time">The time to wait (in milliseconds)</param>
        public Pause(double time) : this(TimeSpan.FromMilliseconds(time))
        { }

        public override async Task ExecuteInstruction()
        {
            Stopwatch sw = Stopwatch.StartNew();
            await Task.Delay(time.Value);
            sw.Stop();

            ElapsedTime.Value = TimeSpan.FromMilliseconds(sw.Elapsed.TotalMilliseconds);
        }
    }
}