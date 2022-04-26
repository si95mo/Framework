namespace Mathematics.DifferentialEquation
{
    /// <summary>
    /// Define a basic prototype of a first order derivative equation
    /// </summary>
    public interface IFirstDerivative
    {
        /// <summary>
        /// Get the actual value
        /// </summary>
        /// <param name="t">The time (in seconds)</param>
        /// <param name="v">The value of the variable</param>
        /// <returns>The calculated derivative value</returns>
        double GetValue(double t, double v);
    }
}