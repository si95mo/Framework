using System.Collections.Generic;
using System.Linq;

namespace Mathematics.Interpolation
{
    /// <summary>
    /// Represent an <see cref="IInterpolation"/> done by using a linear interpolation (i.e. two point straight line equation).
    /// See <see href="https://en.wikipedia.org/wiki/Linear_equation"/> for reference
    /// </summary>
    public class LinearInterpolation : Interpolation
    {
        /// <summary>
        /// Create a new instance of <see cref="LinearInterpolation"/>
        /// </summary>
        public LinearInterpolation() : base()
        { }

        /// <summary>
        /// Apply the linear interpolation
        /// </summary>
        /// <param name="x">The <see cref="IEnumerable{T}"/> of x</param>
        /// <param name="y">THe <see cref="IEnumerable{T}"/> of y</param>
        /// <returns>The <see cref="InterpolationResult"/></returns>
        public override InterpolationResult Interpolate(IEnumerable<double> x, IEnumerable<double> y)
        {
            double meanOfY = y.Average();
            int n = x.Count();

            double x1 = x.ElementAt(0), x2 = x.ElementAt(n - 1);
            double y1 = y.ElementAt(0), y2 = y.ElementAt(n - 1);

            Result.Slope = (y2 - y1) / (x2 - x1);
            Result.Intercept = y1 - Result.Slope * x1;

            // Esteemed values with 2 points straight interpolation
            IEnumerable<double> esteemedValues = x.Select((xi) => Result.Slope * xi + Result.Intercept);

            Result.RSquared = CalculateRSquared(esteemedValues, y, meanOfY);
            Result.AdjustedRSquared = CalculateAdjustedRSquared(Result.RSquared, n); // Adjusted R-squared <= R-squared

            return Result;
        }
    }
}