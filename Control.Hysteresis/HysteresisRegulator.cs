using Core.Parameters;
using Hardware;
using System.Threading.Tasks;

namespace Control.Hysteresis
{
    /// <summary>
    /// Implement an hysteresis <see cref="Regulator"/>
    /// </summary>
    public class HysteresisRegulator : Regulator
    {
        public NumericParameter UpperLimit { get; internal set; }
        public NumericParameter LowerLimit { get; internal set; }
        public TimeSpanParameter CycleTime { get; internal set; }

        private bool doRegulate;
        private Task controlTask;

        /// <summary>
        /// The actuator <see cref="Channel{T}"/>
        /// </summary>
        public Channel<bool> Actuator { get; private set; }

        private bool usePwmInBand;

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
            UpperLimit = new NumericParameter($"{Code}.{nameof(UpperLimit)}", measureUnit: feedbackChannel.MeasureUnit, format: "0.0", value: upperLimit);
            LowerLimit = new NumericParameter($"{Code}.{nameof(LowerLimit)}", measureUnit: feedbackChannel.MeasureUnit, format: "0.0", value: lowerLimit);
            CycleTime = new TimeSpanParameter($"{Code}.{nameof(CycleTime)}", cycleTime);

            doRegulate = false;
            controlTask = null;

            usePwmInBand = false;

            this.Actuator = actuatorChannel;
        }

        /// <summary>
        /// Create the control <see cref="Task"/>
        /// </summary>
        /// <returns>The (async) <see cref="Task"/></returns>
        private Task CreateControlTask()
            => new Task(async () =>
                {
                    while (doRegulate)
                    {
                        if (feedbackChannel.Value > UpperLimit.Value)
                            Actuator.Value = false;
                        else
                        {
                            if (feedbackChannel.Value < LowerLimit.Value)
                                Actuator.Value = true;
                            else
                            {
                                if (usePwmInBand) // 50% PWM, if enabled
                                    Actuator.Value = !Actuator.Value;
                            }
                        }

                        await Task.Delay(CycleTime.Value);
                    }

                    Actuator.Value = false;
                }
            );

        public override void Start()
        {
            usePwmInBand = false;
            StartRegulation();
        }

        /// <summary>
        /// Start the control algorithm and eventually use a 50% PWM inside band, if enabled.
        /// See also <see cref="Start()"/> (that uses no PWM)
        /// </summary>
        /// <param name="usePwmInBand">The PWM option (<see langword="true"/> if PWM has to be used, <see langword="false"/> otherwise)</param>
        public void Start(bool usePwmInBand)
        {
            this.usePwmInBand = usePwmInBand;
            StartRegulation();
        }

        /// <summary>
        /// Start the regulation <see cref="Task"/>
        /// </summary>
        private void StartRegulation()
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