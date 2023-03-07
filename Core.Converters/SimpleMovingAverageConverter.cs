using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Converters
{
    /// <summary>
    /// Convert the input by applying a moving average
    /// </summary>
    public class SimpleMovingAverageConverter : AbstractConverter<double, double>
    {
        private int windowSize;

        /// <summary>
        /// The <see cref="SimpleMovingAverageConverter"/> period
        /// </summary>
        public int WindowSize
        {
            get => windowSize;
            set
            {
                windowSize = value;
                Converter = MovingAverage(windowSize);
            }
        }

        /// <summary>
        /// Initialize a new instance of <see cref="SimpleMovingAverageConverter"/>
        /// </summary>
        /// <param name="windowSize">The moving average window size</param>
        public SimpleMovingAverageConverter(int windowSize) : base()
        {
            this.windowSize = windowSize;
            Converter = MovingAverage(this.windowSize);
        }

        /// <summary>
        /// Calculate the <see cref="SimpleMovingAverageConverter"/>
        /// </summary>
        /// <param name="windowSize">The window size</param>
        /// <returns>The converted value</returns>
        private Func<double, double> MovingAverage(int windowSize)
        {
            // Create a queue with the last [window size] samples
            Queue<double> s = new Queue<double>(windowSize);

            return (x) =>
            {
                // If the queue has reached its max capacity
                if (s.Count >= windowSize)
                    s.Dequeue(); // Dequeue the first element stored

                // Enqueue the new element
                s.Enqueue(x);

                // In the queue are present only the last [window size] element,
                // so the SMA is simply the average of the stored elements
                return s.Average();
            };
        }
    }
}