using System;
using System.Diagnostics;
using System.Threading;

namespace Core.Threading
{
    /// <summary>
    /// Class that provides useful methods for tasks
    /// </summary>
    public class Tasks
    {
        /// <summary>
        /// Perform no operation for the specified amount of time.
        /// Please note that this instruction is CPU-consuming, use with caution!
        /// </summary>
        /// <param name="interval">The time in ms</param>
        public static void NoOperation(int interval)
        {
            var durationTicks = Math.Round((double)(interval * Stopwatch.Frequency)) / 1000;
            var sw = Stopwatch.StartNew();

            while (sw.ElapsedTicks < durationTicks)
            {
                Thread.Sleep(0);
            }
        }
    }
}
