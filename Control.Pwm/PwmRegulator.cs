using Control.PID;
using Core.Parameters;
using Hardware;
using System;
using System.Threading.Tasks;

namespace Control.Pwm
{
    public class PwmRegulator : Regulator
    {
        private Channel<bool> actuatorChannel;
        private bool doRegulate;
        private Task controlTask;

        private PidRegulator pid;

        public NumericParameter MaximumPercentage { get => pid.UpperLimit; internal set => pid.UpperLimit = value; }
        public NumericParameter MinimumPercentage { get => pid.LowerLimit; internal set => pid.LowerLimit = value; }
        public TimeSpanParameter CycleTime { get => pid.CycleTime; internal set => pid.CycleTime = value; }
        public AnalogOutput PwmPercentage => pid.Output;

        public NumericParameter Ton { get; }

        /// <summary>
        /// The regulator output (as percentage of PWM)
        /// </summary>
        public new AnalogOutput Output => pid.Output;

        /// <summary>
        /// The controlled variable (as a generic <see cref="Channel{T}"/>). <br/>
        /// In the block diagram this variable represents r(k) (i.e. the feedback channel)
        /// </summary>
        public Channel<double> Feedback => pid.Feedback;

        /// <summary>
        /// Creste a new instance of <see cref="PwmRegulator"/>
        /// </summary>
        /// <param name="code"></param>
        /// <param name="feedbackChannel">The controlled variable feedback</param>
        /// <param name="actuatorChannel">The actuator <see cref="IChannel"/></param>
        /// <param name="maximumPercentage">The PWM maximum value (in percentage)</param>
        /// <param name="minimumPercentage">The PWM minimum value (in percentage)</param>
        /// <param name="n">The derivative filter coefficient</param>
        /// <param name="kp">The proportional gain</param>
        /// <param name="ki">The integral gain</param>
        /// <param name="kd">The derivative gain</param>
        /// <param name="setpoint">The setpoint</param>
        /// <param name="cycleTime">THe cycle time (in milliseconds)</param>
        public PwmRegulator(string code, Channel<double> feedbackChannel, Channel<bool> actuatorChannel, double maximumPercentage,
            double minimumPercentage, int n, double kp, double ki, double kd, double setpoint, int cycleTime) : base(code, feedbackChannel, setpoint)
        {
            this.actuatorChannel = actuatorChannel;

            pid = new PidRegulator($"{Code}.PID", feedbackChannel, n, kp, ki, kd, maximumPercentage, minimumPercentage, setpoint, cycleTime);

            MaximumPercentage = new NumericParameter($"{Code}.{nameof(MaximumPercentage)}", measureUnit: "", format: "0.0", value: maximumPercentage);
            MinimumPercentage = new NumericParameter($"{Code}.{nameof(MinimumPercentage)}", measureUnit: "", format: "0.0", value: minimumPercentage);
            CycleTime = new TimeSpanParameter($"{Code}.{nameof(CycleTime)}", value: cycleTime);
            Ton = new NumericParameter($"{Code}.Ton", measureUnit: "ms", format: "0.0", value: 0);

            doRegulate = false;
            controlTask = null;
        }

        /// <summary>
        /// Calculate the PWM time
        /// </summary>
        /// <returns>The PWM time (in milliseconds)</returns>
        private double CalculatePwmTime()
        {
            double pwmTime = CycleTime.ValueAsMilliseconds * (pid.Output.Value / 100d);
            return pwmTime;
        }

        /// <summary>
        /// Create the control <see cref="Task"/>
        /// </summary>
        /// <returns>The (async) <see cref="Task"/></returns>
        private Task CreateControlTask() 
            => new Task(async () =>
                {
                    pid.Start();

                    while (true)
                    {
                        Ton.Value = CalculatePwmTime();
                        Ton.Value = Clamp(Ton.Value);

                        actuatorChannel.Value = true;
                        await Task.Delay(TimeSpan.FromMilliseconds(Ton.Value));

                        actuatorChannel.Value = false;
                        await Task.Delay(TimeSpan.FromMilliseconds(CycleTime.ValueAsMilliseconds - Ton.Value));
                    }

                    actuatorChannel.Value = false;
                }
            );

        /// <summary>
        /// Clamp a variable based on <see cref="UpperLimit"/>
        /// and <see cref="LowerLimit"/>
        /// </summary>
        /// <param name="valueToClamp">The value to clamp</param>
        /// <returns>The clamped value</returns>
        private double Clamp(double valueToClamp)
        {
            double result = valueToClamp;

            double maxInPercentage = MaximumPercentage.Value * CycleTime.ValueAsMilliseconds;
            double minInPercentage = MinimumPercentage.Value * CycleTime.ValueAsMilliseconds;

            if (valueToClamp < minInPercentage)
                result = minInPercentage;
            if (valueToClamp > maxInPercentage)
                result = maxInPercentage;

            if (result > 100)
                result = 100;
            if (result < 0)
                result = 0;

            return result;
        }

        public override void Start()
        {
            if (controlTask == null)
            {
                controlTask = CreateControlTask();
                controlTask.Start();
            }
            else
            {
                controlTask.Wait((int)CycleTime.ValueAsMilliseconds);
                controlTask.Dispose();

                controlTask = CreateControlTask();
                controlTask.Start();
            }
        }

        /// <summary>
        /// Reset the <see cref="PwmRegulator"/>
        /// </summary>
        public void Reset() => pid.Reset();
    }
}
