using Core.Parameters;
using Hardware;
using System;

namespace Control
{
    /// <summary>
    /// Define a basic controller
    /// </summary>
    public abstract class Regulator : IRegulator
    {
        protected string code;
        protected Channel<double> feedbackChannel;
        protected AnalogOutput uk;
        protected NumericParameter setpoint;

        /// <summary>
        /// The code
        /// </summary>
        public string Code => code;

        /// <summary>
        /// The value as object
        /// </summary>
        public object ValueAsObject
        {
            get => this;
            set => _ = value;
        }

        /// <summary>
        /// The <see cref="System.Type"/>
        /// </summary>
        public Type Type => typeof(Regulator);

        /// <summary>
        /// The actual setpoint
        /// </summary>
        public NumericParameter Setpoint
        {
            get => setpoint;
            set => setpoint = value;
        }

        /// <summary>
        /// Create a new instance of <see cref="Regulator"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="feedbackChannel">The controlled variable</param>
        /// <param name="setpoint">The desired set point</param>
        protected Regulator(string code, Channel<double> feedbackChannel, double setpoint)
        {
            this.code = code;
            uk = new AnalogOutput($"Y.{code}", measureUnit: feedbackChannel.MeasureUnit, format: feedbackChannel.Format);
            this.feedbackChannel = feedbackChannel;
            this.setpoint = new NumericParameter("Setpoint", value: setpoint, measureUnit: feedbackChannel.MeasureUnit, format: feedbackChannel.Format);
        }

        public abstract void Start();
    }
}