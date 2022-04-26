namespace Mathematics.DifferentialEquation
{
    /// <summary>
    /// Implement an <see cref="IFirstDerivative"/> solver that uses Runge-Kutta
    /// </summary>
    public class Solver
    {
        private IFirstDerivative derivative;
        private double time, value, step;
        private double delta;

        /// <summary>
        /// Create a new instance of <see cref="Solver"/>
        /// </summary>
        /// <param name="derivative">The <see cref="IFirstDerivative"/></param>
        /// <param name="initialTime">The initial time</param>
        /// <param name="initialValue">The initial value</param>
        /// <param name="step">The step</param>
        public Solver(IFirstDerivative derivative, double initialTime, double initialValue, double step)
        {
            this.derivative = derivative;
            time = initialTime;
            value = initialValue;
            this.step = step;

            delta = derivative.GetValue(initialTime, initialValue);
        }

        /// <summary>
        /// Perform a step of the Runge-Kutta algorithm
        /// </summary>
        /// <param name="time">The actual time</param>
        /// <param name="value">The actual value</param>
        /// <returns>The calculated delta of the <see cref="IFirstDerivative"/></returns>
        public double Step(out double time, out double value)
        {
            double k1 = delta;
            double k2 = derivative.GetValue(this.time + step / 2, this.value + step / 2 * k1);
            double k3 = derivative.GetValue(this.time + step / 2, this.value + step / 2 * k2);
            double k4 = derivative.GetValue(this.time + step, this.value + step * k3);

            value = this.value + step / 6 * (k1 + 2 * k2 + 2 * k3 + k4);
            time = this.time + step;
            delta = derivative.GetValue(time, value);

            this.time = time;
            this.value = value;

            return delta;
        }
    }
}