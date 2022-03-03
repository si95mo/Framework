using Core.Parameters;
using Hardware;
using System;
using System.Threading.Tasks;

namespace Control.Hysteresis
{
    /// <summary>
    /// Implement an hysteresis <see cref="Regulator"/>
    /// </summary>
    public class HysteresisRegulator : Regulator
    {
        public NumericParameter UpperLimit;
        public NumericParameter LowerLimit;
        public TimeSpanParameter CycleTime;

        private Channel<bool> actuatorChannel;
        private bool doRegulate;
        private Task controlTask;

        /// <summary>
        /// Create a new instance of <see cref="HysteresisRegulator"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="feedbackChannel">The feedback <see cref="Channel{T}"/></param>
        /// <param name="actuatorChannel">The actuator <see cref="Channel{T}"/></param>
        /// <param name="upperLimit">The upper limit</param>
        /// <param name="lowerLimit">The lower limit</param>
        /// <param name="setpoint">The setpoint</param>
        /// <param name="cycleTime">The cycle time (in milliseconds)</param>
        public HysteresisRegulator(string code, Channel<double> feedbackChannel, Channel<bool> actuatorChannel,
            double upperLimit, double lowerLimit, double setpoint, double cycleTime) : base(code, feedbackChannel, setpoint)
        {
            UpperLimit = new NumericParameter(nameof(UpperLimit), upperLimit, measureUnit: feedbackChannel.MeasureUnit, format: feedbackChannel.Format);
            LowerLimit = new NumericParameter(nameof(LowerLimit), lowerLimit, measureUnit: feedbackChannel.MeasureUnit, format: feedbackChannel.Format);
            CycleTime = new TimeSpanParameter(nameof(CycleTime), TimeSpan.FromMilliseconds(cycleTime));

            this.actuatorChannel = actuatorChannel;

            doRegulate = false;
            controlTask = null;
        }

        /// <summary>
        /// Create the control <see cref="Task"/>
        /// </summary>
        /// <returns>The (async) <see cref="Task"/></returns>
        private Task CreateControlTask() => new Task(async () =>
             {
                 while (doRegulate)
                 {
                     if (feedbackChannel.Value > UpperLimit.Value)
                         actuatorChannel.Value = false;
                     else
                     {
                         if (feedbackChannel.Value < LowerLimit.Value)
                             actuatorChannel.Value = true;
                     }

                     await Task.Delay(CycleTime.Value);
                 }
             }
        );

        public override void Start()
        {
            if (!doRegulate)
                doRegulate = true;

            if (controlTask == null)
            {
                controlTask = CreateControlTask();
                controlTask.Start();
            }
            else
            {
                controlTask.Wait((int)CycleTime.Value.TotalMilliseconds);
                controlTask.Dispose();

                controlTask = CreateControlTask();
                controlTask.Start();
            }
        }

        public void Stop()
        {
            doRegulate = false;
        }
    }
}