using System;

namespace Core.Converters
{
    /// <summary>
    /// Implement an <see cref="AbstractConverter{TIn, TOut}"/> that perform
    /// an online mean based on the input values
    /// </summary>
    public class OnlineMeanConverter : AbstractConverter<double, double>
    {
        private int n;
        private double lastMean;

        /// <summary>
        /// Create a new instance of <see cref="OnlineMeanConverter"/>
        /// </summary>
        public OnlineMeanConverter() : base()
        {
            n = 0;
            lastMean = 0d;

            converter = new Func<double, double>(x => CalculateOnlineMean(x));
        }

        /// <summary>
        /// Calculate the online mean
        /// </summary>
        /// <param name="x">The value to add to the mean</param>
        private double CalculateOnlineMean(double x)
        {
            double mean = (lastMean * n + x) / ++n;
            lastMean = mean;

            return mean;
        }
    }
}