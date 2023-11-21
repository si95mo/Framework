using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Core
{
    /// <summary>
    /// Implement a debounce logic that can be applied on event handlers
    /// </summary>
    public class Debouncer : IDisposable
    {
        private readonly TimeSpan debounceTime;
        private readonly HashSet<ManualResetEvent> resets;
        private readonly object sync;

        /// <summary>
        /// Create a new instance of <see cref="Debouncer"/>
        /// </summary>
        /// <param name="debounceTime">The debounce time (as a <see cref="TimeSpan"/>)</param>
        public Debouncer(TimeSpan debounceTime)
        {
            this.debounceTime = debounceTime;

            resets = new HashSet<ManualResetEvent>();
            sync = new object();
        }

        /// <summary>
        /// Create a new instance of <see cref="Debouncer"/>
        /// </summary>
        /// <param name="debounceTimeInMilliseconds">The debounce time in milliseconds</param>
        public Debouncer(double debounceTimeInMilliseconds) : this(TimeSpan.FromMilliseconds(debounceTimeInMilliseconds)) 
        { }

        /// <summary>
        /// Invoke the action if the debounce logic applies. <br/>
        /// For example in an event handler: <c>deboucer.Invoke(() => MethodToCall(e))</c>
        /// </summary>
        public void Invoke(Action action)
        {
            ManualResetEvent reset = new ManualResetEvent(false);

            lock (sync)
            {
                while (resets.Count > 0)
                {
                    ManualResetEvent otherReset = resets.First();

                    resets.Remove(otherReset);
                    otherReset.Set();
                }

                resets.Add(reset);
            }

            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    if (!reset.WaitOne(debounceTime))
                    {
                        action();
                    }
                }
                finally
                {
                    lock (sync)
                    {
                        using (reset)
                        {
                            resets.Remove(reset);
                        }
                    }
                }
            }
            );
        }

        public void Dispose()
        {
            lock (sync)
            {
                while (resets.Count > 0)
                {
                    ManualResetEvent reset = resets.First();

                    resets.Remove(reset);
                    reset.Set();
                }
            }
        }
    }
}
