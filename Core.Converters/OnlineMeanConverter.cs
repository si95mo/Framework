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
        /// <param name="onValueChange">The option to trigger the update on value change or set event</param>
        public OnlineMeanConverter(bool onValueChange = true) : base(onValueChange)
        {
            n = 0;
            lastMean = 0d;

            Converter = new Func<double, double>(x => CalculateOnlineMean(x));
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