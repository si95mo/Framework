using System.Collections.Generic;
using System.Diagnostics;

namespace Diagnostic
{
    /// <summary>
    /// Provide useful time-related instructions
    /// </summary>
    public static class Timer
    {
        private static Queue<Stopwatch> timers;
        private static Queue<double> elapsedMillisecods;

        /// <summary>
        /// The first stored elapsed time in milliseconds
        /// </summary>
        /// <remarks>
        /// If no time value is stored, this property will contain
        /// <see langword="0d"/> as its default value
        /// </remarks>
        public static double ElapsedTime
        {
            get
            {
                double elapsed = 0d;

                if (elapsedMillisecods.Count > 0)
                    elapsed = elapsedMillisecods.Peek();

                return elapsed;
            }
        }

        /// <summary>
        /// Initialize the <see cref="Timer"/> class
        /// </summary>
        public static void Initialize()
        {
            timers = new Queue<Stopwatch>();
            elapsedMillisecods = new Queue<double>();
        }

        /// <summary>
        /// Start a new time counter
        /// </summary>
        public static void Start()
            => timers.Enqueue(Stopwatch.StartNew());

        /// <summary>
        /// Stop the first active time counter and retrieve its elapsed time in milliseconds
        /// </summary>
        /// <remarks>
        /// This method stop the actual running timer and enqueue its associated elapsed time, so do <b>not</b>
        /// call <see cref="ElapsedTime"/> if you are interested in the actual elapsed time associated to the running timer
        /// and use <see cref="ElapsedTime"/> for the elapsed time (in milliseconds)
        /// </remarks>
        public static void Stop()
        {
            Stopwatch timer = timers.Dequeue();
            timer.Stop();

            elapsedMillisecods.Enqueue(timer.Elapsed.TotalMilliseconds);
        }

        /// <summary>
        /// Stop the first active time counter and retrieve its elapsed time
        /// </summary>
        /// <returns>The elapsed time (in milliseconds)</returns>
        /// <remarks>
        /// This method internally call the <see cref="Stop"/> method, so do <b>not</b>
        /// call it if you are interested in the actual elapsed time associated to the running timer
        /// </remarks>
        public static double GetElapsedTime()
        {
            Stop();
            double elapsed = elapsedMillisecods.Dequeue();

            return elapsed;
        }
    }
}