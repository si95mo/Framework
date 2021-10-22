using System;

namespace Core.Converters
{
    /// <summary>
    /// Convert the input by applying an exponential moving average
    /// </summary>
    public class ExponentialMovingAverageConverter : AbstractConverter<double, double>
    {
        private double forgettingFactor;

        /// <summary>
        /// The <see cref="ExponentialMovingAverageConverter"/> forgetting factor
        /// </summary>
        public double ForgettingFactor
        {
            get => forgettingFactor;
            set
            {
                forgettingFactor = value <= 1 && value >= 0 ? value : forgettingFactor;
                converter = MovingAverage(forgettingFactor);
            }
        }

        /// <summary>
        /// Initialize a new instance of <see cref="ExponentialMovingAverageConverter"/>
        /// </summary>
        /// <param name="forgettingFactor">The forgetting factor</param>
        public ExponentialMovingAverageConverter(double forgettingFactor) : base()
        {
            this.forgettingFactor = forgettingFactor;
            converter = MovingAverage(this.forgettingFactor);
        }

        /// <summary>
        /// Calculate the <see cref="ExponentialMovingAverageConverter"/>
        /// </summary>
        /// <param name="lambda">The forgetting factor</param>
        /// <returns>The converted value</returns>
        private Func<double, double> MovingAverage(double lambda)
        {
            double ema = 0, emaOld = 0;
            double w = 1, wOld = 1;

            return (x) =>
            {
                w = lambda * wOld + 1;
                wOld = w;

                ema = x * (1 / w) + emaOld * (1 - (1 / w));
                emaOld = ema;

                return ema;
            };
        }
    }
}