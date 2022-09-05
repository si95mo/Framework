using System.Collections.Generic;

namespace Mathematics.Interpolation
{
    /// <summary>
    /// Define the result of an <see cref="IInterpolation"/>
    /// </summary>
    public struct InterpolationResult
    {
        /// <summary>
        /// The slope (m)
        /// </summary>
        public double Slope;

        /// <summary>
        /// The intercept (q)
        /// </summary>
        public double Intercept;

        /// <summary>
        /// The R-squared
        /// </summary>
        public double RSquared;

        /// <summary>
        /// The adjusted R-squared (one independent variable)
        /// </summary>
        /// <remarks>Adjusted R-squared will always be less or equal to R-squared</remarks>
        public double AdjustedRSquared;

        /// <summary>
        /// Create a new instance of <see cref="LinearRegressionResult"/>
        /// </summary>
        /// <param name="slope">The slope</param>
        /// <param name="intercept">The intercept</param>
        /// <param name="rSquared">The R-squared</param>
        /// <param name="adjustedRSquared">The adjusted R-squared</param>
        public InterpolationResult(double slope, double intercept, double rSquared, double adjustedRSquared)
        {
            Slope = slope;
            Intercept = intercept;
            RSquared = rSquared;
            AdjustedRSquared = adjustedRSquared;
        }

        public override string ToString()
            => $"y = {Slope:0.000} * x + {Intercept:0.000}";
    }

    /// <summary>
    /// Define a generic interpolation
    /// </summary>
    public interface IInterpolation
    {
        /// <summary>
        /// The slope (m)
        /// </summary>
        double Slope { get; }

        /// <summary>
        /// The intercept (q)
        /// </summary>
        double Intercept { get; }

        /// <summary>
        /// The R-squared
        /// </summary>
        double RSquared { get; }

        /// <summary>
        /// The adjusted R-squared (one independent variable)
        /// </summary>
        double AdjustedRSquared { get; }

        /// <summary>
        /// Perform the interpolation
        /// </summary>
        /// <param name="x">The x values</param>
        /// <param name="y">The y values</param>
        /// <returns>The <see cref="InterpolationResult"/></returns>
        InterpolationResult Interpolate(IEnumerable<double> x, IEnumerable<double> y);
    }
}