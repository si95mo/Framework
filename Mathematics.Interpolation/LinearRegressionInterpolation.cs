using System.Collections.Generic;
using System.Linq;

namespace Mathematics.Interpolation
{
    /// <summary>
    /// Perform an <see cref="IInterpolation"/> done by applying a linear regression (i.e. best fit interpolation).
    /// See <see href="https://en.wikipedia.org/wiki/Simple_linear_regression"/> and <see href="https://en.wikipedia.org/wiki/Coefficient_of_determination"/> for reference
    /// </summary>
    public class LinearRegressionInterpolation : Interpolation
    {
        /// <summary>
        /// Create a new instance of <see cref="LinearRegressionInterpolation"/>
        /// </summary>
        public LinearRegressionInterpolation() : base()
        { }

        /// <summary>
        /// Apply the linear regression
        /// </summary>
        /// <param name="x">The <see cref="IEnumerable{T}"/> of x</param>
        /// <param name="y">THe <see cref="IEnumerable{T}"/> of y</param>
        /// <returns>The <see cref="InterpolationResult"/></returns>
        public override InterpolationResult Interpolate(IEnumerable<double> x, IEnumerable<double> y)
        {
            int n = x.Count(); // Number of elements

            // Mean values
            double meanOfX = x.Average();
            double meanOfY = y.Average();

            IEnumerable<double> xDifferences = x.Select((xi) => xi - meanOfX); // xi - mean(x)
            IEnumerable<double> yDifferences = y.Select((yi) => yi - meanOfY); // yi - mean(y)

            // m = sum(xi * yi) / sum(xi^2)
            Result.Slope = xDifferences.Zip(yDifferences, (xi, yi) => xi * yi).Sum() / xDifferences.Select((xi) => xi * xi).Sum();
            // q = mean(y) - m * mean(x)
            Result.Intercept = meanOfY - Result.Slope * meanOfX;

            // Esteemed values with linear regression
            IEnumerable<double> esteemedValues = x.Select((xi) => Result.Slope * xi + Result.Intercept);

            Result.RSquared = CalculateRSquared(esteemedValues, y, meanOfY);
            Result.AdjustedRSquared = CalculateAdjustedRSquared(Result.RSquared, n); // Adjusted R-squared <= R-squared

            return Result;
        }
    }
}