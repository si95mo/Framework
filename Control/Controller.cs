using Hardware;
using System;

namespace Control
{
    /// <summary>
    /// Define a basic controller
    /// </summary>
    public abstract class Controller : IController
    {
        protected string code;
        protected Channel<double> controlledVariable;
        protected AnalogOutput uk;
        protected double setPoint;

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
        public Type Type => typeof(Controller);

        /// <summary>
        /// The actual set point
        /// </summary>
        public double SetPoint
        {
            get => setPoint;
            set => setPoint = value;
        }

        /// <summary>
        /// Create a new instance of <see cref="Controller"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="controlledVariable">The controlled variable</param>
        /// <param name="setPoint">The desired set point</param>
        protected Controller(string code, Channel<double> controlledVariable, double setPoint)
        {
            this.code = code;
            uk = new AnalogOutput($"Y.{code}", measureUnit: controlledVariable.MeasureUnit, format: controlledVariable.Format);
            this.controlledVariable = controlledVariable;
            this.setPoint = setPoint;
        }

        public abstract void Start();
    }
}