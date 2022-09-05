using System;
using System.Collections.Generic;
using System.Linq;

namespace Mathematics.Interpolation
{
    /// <summary>
    /// Define an abstract <see cref="IInterpolation"/>
    /// </summary>
    public abstract class Interpolation : IInterpolation
    {
        protected InterpolationResult Result;

        public double Slope => Result.Slope;
        public double Intercept => Result.Intercept;
        public double RSquared => Result.RSquared;
        public double AdjustedRSquared => Result.AdjustedRSquared;

        /// <summary>
        /// Initialize the <see cref="Interpolation"/> attributes
        /// </summary>
        protected Interpolation()
            => Result = new InterpolationResult();

        public abstract InterpolationResult Interpolate(IEnumerable<double> x, IEnumerable<double> y);

        /// <summary>
        /// Calculate the R-squared value
        /// </summary>
        /// <param name="esteemedValues">The esteemed value after interpolation</param>
        /// <param name="y">The y values</param>
        /// <param name="meanOfY">The mean value of <paramref name="y"/></param>
        /// <returns>The R-squared</returns>
        protected double CalculateRSquared(IEnumerable<double> esteemedValues, IEnumerable<double> y, double meanOfY)
        {
            IEnumerable<double> esteemedDifferences = esteemedValues.Zip(y, (yi, yiEsteemed) => Math.Pow(yi - yiEsteemed, 2)); // (ti - esteemed ti)^2
            IEnumerable<double> yDifferencesSquared = y.Select((yi) => yi - meanOfY).Select((yi) => Math.Pow(yi, 2)); // yi^2

            double rSquared = 1 - (esteemedDifferences.Sum() / yDifferencesSquared.Sum()); // 1 - sum((yi - esteemed yi)^2) / sum(yi^2)
            return rSquared;
        }

        /// <summary>
        /// Calculate the adjusted R-squared value
        /// </summary>
        /// <remarks>Adjusted R-squared will always be less or equal to R-squared</remarks>
        /// <param name="rSquared">The R-squared</param>
        /// <param name="n">The sample length</param>
        /// <returns>The adjusted R-squared</returns>
        protected double CalculateAdjustedRSquared(double rSquared, int n)
        {
            double adjustedRSquared = 1 - ((n - 1) / (n - 1 - 1) * (1 - rSquared)); // Adjusted R-squared <= R-squared
            return adjustedRSquared;
        }
    }
}