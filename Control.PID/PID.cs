using Core.Parameters;
using Core.Threading;
using Hardware;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Control.PID
{
    /// <summary>
    /// Implement a (P)roportional (I)ntegrative (D)erivative controller
    /// </summary>
    public class PID : Regulator
    {
        private int n;
        private NumericParameter kp, ki, kd;
        private NumericParameter upperLimit, lowerLimit;
        private TimeSpanParameter cycleTime;

        private TimeSpan timeSinceLastUpdate;

        private NumericParameter proportionalTerm;
        private NumericParameter integralTerm;
        private NumericParameter derivativeTerm;

        private Task controlTask;

        /// <summary>
        /// The filter coefficient
        /// </summary>
        public int N { get => n; set => n = value; }

        /// <summary>
        /// The proportional gain
        /// </summary>
        public NumericParameter Kp { get => kp; set => kp = value; }

        /// <summary>
        /// The integral gain
        /// </summary>
        public NumericParameter Ki { get => ki; set => ki = value; }

        /// <summary>
        /// The derivative gain
        /// </summary>
        public NumericParameter Kd { get => kd; set => kd = value; }

        /// <summary>
        /// The clamping upper output limit of the controller
        /// </summary>
        public NumericParameter UpperLimit { get => upperLimit; set => upperLimit = value; }

        /// <summary>
        /// The clamping lower output limit of the controller
        /// </summary>
        public NumericParameter LowerLimit { get => lowerLimit; set => lowerLimit = value; }

        /// <summary>
        /// The controlled variable (as a generic <see cref="Channel{T}"/>). <br/>
        /// In the block diagram this variable represents r(k)
        /// </summary>
        public Channel<double> Rk => feedbackChannel;

        /// <summary>
        /// The cycle time of the controller (in milliseconds)
        /// </summary>
        public TimeSpanParameter CycleTime
        {
            get => cycleTime;
            set => cycleTime = value;
        }

        /// <summary>
        /// Create a new instance of <see cref="PID"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="feedback">The controlled variable feedback</param>
        /// <param name="n">The derivative filter coefficient</param>
        /// <param name="kp">The proportional gain</param>
        /// <param name="ki">The integral gain</param>
        /// <param name="kd">The derivative gain</param>
        /// <param name="upperLimit">The upper limit (for clamping)</param>
        /// <param name="lowerLimit">The lower limit (for clamping)</param>
        /// <param name="setpoint">The desired setpoint</param>
        public PID(string code, Channel<double> feedback, int n, double kp, double ki, double kd,
            double upperLimit, double lowerLimit, double setpoint) : base(code, feedback, setpoint)
        {
            this.n = n;
            this.kp = new NumericParameter($"{Code}.Kp", value: kp, format: "0.000");
            this.ki = new NumericParameter($"{Code}.Ki", value: ki, format: "0.000");
            this.kd = new NumericParameter($"{Code}.Kd", value: kd, format: "0.000");
            this.upperLimit = new NumericParameter($"{Code}.UpperLimit", value: upperLimit, measureUnit: feedback.MeasureUnit, format: feedback.Format);
            this.lowerLimit = new NumericParameter($"{Code}.LowerLimit", value: lowerLimit, measureUnit: feedback.MeasureUnit, format: feedback.Format);

            proportionalTerm = new NumericParameter($"{Code}.ProportionalTerm", format: "0.000");
            integralTerm = new NumericParameter($"{Code}.IntegralTerm", format: "0.000");
            derivativeTerm = new NumericParameter($"{Code}.DerivativeTerm", format: "0.000");

            controlTask = null;
        }

        /// <summary>
        /// Create a new controlling <see cref="Task"/>
        /// </summary>
        /// <returns>The controlling <see cref="Task"/></returns>
        private Task CreateControlTask() => new Task(async () =>
                {
                    Stopwatch sw;
                    int timeToWait;

                    timeSinceLastUpdate = new TimeSpan(0);
                    while (true)
                    {
                        sw = Stopwatch.StartNew();

                        Iterate();

                        timeToWait = (int)(cycleTime.Value.TotalMilliseconds - sw.Elapsed.TotalMilliseconds);
                        if (timeToWait > 0)
                            await Tasks.NoOperation(timeToWait, 1);

                        timeSinceLastUpdate = new TimeSpan(sw.Elapsed.Ticks).Subtract(timeSinceLastUpdate);
                    }
                }
        );

        /// <summary>
        /// Start the PID controller (or restart it if already started)
        /// </summary>
        public override void Start()
        {
            if (controlTask == null)
            {
                controlTask = CreateControlTask();
                controlTask.Start();
            }
            else
            {
                controlTask.Wait((int)cycleTime.Value.TotalMilliseconds);
                controlTask.Dispose();

                controlTask = CreateControlTask();
                controlTask.Start();
            }
        }

        /// <summary>
        /// Perform an iteration of the control algorithm
        /// </summary>
        private void Iterate()
        {
            double error = setpoint.Value - feedbackChannel.Value;

            // Integral term
            integralTerm.Value += ki.Value * error * timeSinceLastUpdate.TotalSeconds;
            integralTerm.Value = Clamp(integralTerm.Value);

            // Derivative term
            // double dInput = u.Value - lastControlledValue;
            derivativeTerm.Value = kd.Value * (n / (1 + n * integralTerm.Value / ki.Value));

            // Proportional term
            proportionalTerm.Value = kp.Value * error;

            // Output update
            uk.Value = proportionalTerm.Value + integralTerm.Value - derivativeTerm.Value;
            uk.Value = Clamp(uk.Value);
        }

        /// <summary>
        /// Clamp a variable based on <see cref="UpperLimit"/>
        /// and <see cref="LowerLimit"/>
        /// </summary>
        /// <param name="valueToClamp">The value to clamp</param>
        /// <returns>The clamped value</returns>
        private double Clamp(double valueToClamp)
        {
            double result = valueToClamp;

            if (valueToClamp <= lowerLimit.Value)
                result = lowerLimit.Value;
            if (valueToClamp >= upperLimit.Value)
                result = upperLimit.Value;

            return result;
        }

        /// <summary>
        /// Reset the <see cref="PID"/> controller by setting the
        /// integral term to 0 and the last update time to now
        /// </summary>
        public void Reset()
        {
            integralTerm.Value = 0;
            timeSinceLastUpdate = new TimeSpan(0);
        }
    }
}