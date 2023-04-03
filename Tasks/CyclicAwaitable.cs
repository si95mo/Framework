using Diagnostic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    /// <summary>
    /// Define a cyclic version of an <see cref="Awaitable"/> task that will run <see cref="Awaitable.Execution"/> inside a timed loop
    /// </summary>
    public abstract class CyclicAwaitable : Awaitable
    {
        /// <summary>
        /// The cycle time <see cref="TimeSpan"/>
        /// </summary>
        public TimeSpan CycleTime { get; protected set; }

        /// <summary>
        /// Create a new instance of <see cref="CyclicAwaitable"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="cycleTimeInMilliseconds">The cycle time in milliseconds</param>
        /// <param name="scheduler">The <see cref="Scheduler"/></param>
        protected CyclicAwaitable(string code, int cycleTimeInMilliseconds, Scheduler scheduler = null)
            : this(code, TimeSpan.FromMilliseconds(cycleTimeInMilliseconds), scheduler)
        { }

        /// <summary>
        /// Create a new instance of <see cref="CyclicAwaitable"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="cycleTime">The cycle time <see cref="TimeSpan"/></param>
        /// <param name="scheduler">The <see cref="Scheduler"/></param>
        protected CyclicAwaitable(string code, TimeSpan cycleTime, Scheduler scheduler = null) : base(code, scheduler)
        {
            CycleTime = cycleTime;
        }

        public override Task Start()
        {
            StopRequested = false;

            TokenSource.Dispose();
            TokenSource = new CancellationTokenSource();

            Status.Value = TaskStatus.WaitingToRun;

            Task task = new Task(() =>
                {
                    Status.Value = TaskStatus.Running;

                    try
                    {
                        // Iterate through the Execution task state
                        bool exitCycle = false;
                        ManualResetEventSlim stopRequest = new ManualResetEventSlim(false);

                        Stopwatch timer;
                        IEnumerable<string> executionState;
                        while (!exitCycle)
                        {
                            timer = Stopwatch.StartNew();

                            do
                            {
                                executionState = Execution();
                                foreach (string state in executionState)
                                {
                                    // Exit loop if task has been canceled
                                    if (TokenSource.Token.IsCancellationRequested)
                                    {
                                        exitCycle = true; // Exit timed while when cancellation is requested
                                        break;
                                    }

                                    WaitState.Value = state;
                                }
                            } while (stopRequest.Wait((int)Math.Max(0, CycleTime.TotalMilliseconds - timer.ElapsedMilliseconds)));

                            timer.Stop();
                            if (timer.ElapsedMilliseconds > CycleTime.TotalMilliseconds + 10) // Plus 10ms of threshold
                                Logger.Warn($"{Code} cycle time of {CycleTime.TotalMilliseconds} [ms] exceeded. Last cycle took {timer.ElapsedMilliseconds} [ms]");
                        }

                        // And then through the termination task state (that should be canceled)
                        IEnumerable terminationState = Termination();
                        foreach (string state in terminationState)
                            WaitState.Value = state;

                        Status.Value = TaskStatus.RanToCompletion;
                    }
                    catch (Exception ex) // This may be caused by a stop request or an actual exception
                    {
                        Status.Value = TaskStatus.Faulted;

                        if (StopRequested)
                            Logger.Error(ex);
                        else
                            Logger.Warn($"Task with code {Code} faulted because a stop has been requested");
                    }
                },
                TokenSource.Token
            );

            task.Start(Scheduler);
            return task;
        }
    }
}