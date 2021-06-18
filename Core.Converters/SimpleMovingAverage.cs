using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Converters
{
    /// <summary>
    /// Convert the input by applying a moving average 
    /// </summary>
    public class SimpleMovingAverage : AbstractConverter<double, double>
    {
        private int period;

        /// <summary>
        /// The <see cref="SimpleMovingAverage"/> period
        /// </summary>
        public int Period
        {
            get => period;
            set => period = value;
        }

        /// <summary>
        /// Initialize a new instance of <see cref="SimpleMovingAverage"/>
        /// </summary>
        /// <param name="period">The period</param>
        public SimpleMovingAverage(int period) : base()
        {
            converter = MovingAverage(period);
        }

        /// <summary>
        /// Calculate the <see cref="SimpleMovingAverage"/>
        /// </summary>
        /// <param name="period">The period</param>
        /// <returns>The converted value</returns>
        private Func<double, double> MovingAverage(int period)
        {
            Queue<double> s = new Queue<double>(period);

            return (x) =>
            {
                if (s.Count >= period)
                    s.Dequeue();

                s.Enqueue(x);

                return s.Average();
            };
        }
    }
}
