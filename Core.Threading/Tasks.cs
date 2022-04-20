using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Threading
{
    /// <summary>
    /// Class that provides useful methods for tasks
    /// </summary>
    public class Tasks
    {
        /// <summary>
        /// Perform no operation for the specified amount of time.
        /// </summary>
        /// <param name="interval">The interval (in milliseconds)</param>
        /// <remarks>
        /// This implementation has a precision of about 15.6ms. <br/>
        /// For a more precise timing, see <see cref="NoOperation(int, uint)"/>
        /// </remarks>
        public static async Task NoOperation(int interval)
        {
            ManualResetEventSlim stopRequest = new ManualResetEventSlim(false);

            Stopwatch sw = Stopwatch.StartNew();
            do
            {
                await Task.Delay(1);
            } while (stopRequest.Wait((int)Math.Max(0, interval - sw.ElapsedMilliseconds)));
        }

        /// <summary>
        /// Perform no operation for the specified amount of time and, while doing
        /// no operation, set the internal <paramref name="clockRate"/>. <br/>
        /// See <see cref="ThreadingApi.SetTimeBeginPeriod(uint)"/> and
        /// <see cref="ThreadingApi.SetTimeEndPeriod(uint)"/>
        /// </summary>
        /// <param name="interval">The interval (in milliseconds)</param>
        /// <param name="clockRate">The Windows interrupt clock rate (in milliseconds)</param>
        /// <remarks>
        /// The new clock rate is reseted at the end of the method
        /// (at approximatively 15.6ms, <see cref="NoOperation(int)"/>)
        /// </remarks>
        public static async Task NoOperation(int interval, uint clockRate = 15)
        {
            ThreadingApi.SetTimeBeginPeriod(clockRate);
            await NoOperation(interval);
            ThreadingApi.SetTimeEndPeriod(clockRate);
        }
    }
}