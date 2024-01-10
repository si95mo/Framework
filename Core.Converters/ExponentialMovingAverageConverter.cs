using System;

namespace Core.Converters
{
    /// <summary>
    /// Convert the input by applying an exponential moving average. <br/>
    /// For reference, see:
    /// <see href="https://it.mathworks.com/help/dsp/ug/sliding-window-method-and-exponential-weighting-method.html"/>
    /// </summary>
    public class ExponentialMovingAverageConverter : AbstractConverter<double, double>
    {
        private double lambda;

        /// <summary>
        /// The <see cref="ExponentialMovingAverageConverter"/> forgetting factor
        /// </summary>
        public double Lambda
        {
            get => lambda;
            set
            {
                lambda = value <= 1 && value >= 0 ? value : lambda;
            }
        }

        /// <summary>
        /// Initialize a new instance of <see cref="ExponentialMovingAverageConverter"/>
        /// </summary>
        /// <remarks>If <paramref name="lambda"/> is not between 0 and 1, a value of 0.5 is assigned by default</remarks>
        /// <param name="lambda">The forgetting factor (in 0 and 1, inclusive)</param>
        /// <param name="onValueChange">The option to trigger the update on value change or set event</param>
        public ExponentialMovingAverageConverter(double lambda, bool onValueChange = true) : base(onValueChange)
        {
            this.lambda = lambda <= 1 && lambda >= 0 ? lambda : 0.5;
            Converter = MovingAverage();
        }

        /// <summary>
        /// Calculate the <see cref="ExponentialMovingAverageConverter"/>
        /// </summary>
        /// <returns>The converted value</returns>
        private Func<double, double> MovingAverage()
        {
            double ema = 0, emaOld = 0;
            double w = 1, wOld = 1;

            // w[k] = (lambda) * w[k - 1] + 1
            // EMA[k] = (1 - 1 / w[k]) * EMA[k - 1] + (1 / w[k]) * x[k]
            // Where:
            // - lambda is the forgetting factor
            // - w[k] and w[k - 1] are the weighting factors (current and old, respectively)
            // - EMA[k] and EMA[k - 1] are the result of the exponential moving average (current and old)
            // - x[k] is the current input sample
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