using System;
using System.Diagnostics;
using System.Threading;

namespace Extensions
{
    /// <summary>
    /// Provide logic structure-related extension methods
    /// </summary>
    public static class LoopExtensions
    {
        /// <summary>
        /// Provide a timed <see langword="while"/> loop
        /// </summary>
        /// <param name="source">The <see cref="Action"/> to perform on each step</param>
        /// <param name="condition">The <see cref="Func{T, TResult}"/> that represent the loop condition</param>
        /// <param name="interval">The timed loop interval (in milliseconds)</param>
        public static void TimedWhile(this Action source, Func<bool> condition, int interval)
        {
            ManualResetEventSlim stopRequest = new ManualResetEventSlim(false);
            Stopwatch stopwatch;

            while (condition())
            {
                stopwatch = Stopwatch.StartNew();

                do
                    source();
                while (stopRequest.Wait((int)Math.Max(0, interval - stopwatch.ElapsedMilliseconds)));

                stopwatch.Stop();
            }
        }
    }
}