using Core.Converters;
using Core.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    /// <summary>
    /// Define a <see cref="Task"/> <see cref="IScheduler"/>
    /// </summary>
    public class Scheduler : TaskScheduler, IScheduler
    {
        #region Private attributes

        // The maximum concurrency level allowed by this scheduler.
        public int MaxDeegreesOfParallelism { get; private set; }

        // A list of tasks that are currently scheduled or running.
        private readonly LinkedList<Task> tasks;

        // Whether the current thread is processing tasks.
        [ThreadStatic]
        private static bool currentThreadIsProcessingTasks;

        #endregion Private attributes

        #region IProperty implementation

        public string Code { get; }

        public object ValueAsObject { get; set; }

        public Type Type => GetType();

        #endregion IProperty implementation

        #region IScheduler implementation

        public NumericParameter RunningTasks { get; protected set; }
        public NumericParameter Load { get; protected set; }

        #endregion IScheduler implementation

        #region Constructors

        /// <summary>
        /// Create a new instance of <see cref="Scheduler"/>
        /// </summary>
        /// <param name="maxDegreesOfParallelism">The maximum degrees of parallelism</param>
        public Scheduler(int maxDegreesOfParallelism = 100) : this(Guid.NewGuid().ToString(), maxDegreesOfParallelism)
        { }

        /// <summary>
        /// Create a new instance of <see cref="Scheduler"/>
        /// </summary>
        /// <param name="code"></param>
        /// <param name="maxDegreesOfParallelism">The maximum degrees of parallelism</param>
        public Scheduler(string code, int maxDegreesOfParallelism = 100)
        {
            Code = code;
            MaxDeegreesOfParallelism = maxDegreesOfParallelism;

            RunningTasks = new NumericParameter($"{Code}.{nameof(RunningTasks)}", measureUnit: string.Empty, format: "0", value: 0d);
            Load = new NumericParameter($"{Code}.{nameof(Load)}", measureUnit: "%", format: "0.00", value: 0d);

            RunningTasks.ConnectTo(Load, new GenericConverter<double, double>(new Func<double, double>((x) => x / MaxDeegreesOfParallelism * 100d)));

            tasks = new LinkedList<Task>();
        }

        #endregion Constructors

        #region TaskScheduler implementation

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(tasks, ref lockTaken);

                if (lockTaken)
                    return tasks.ToArray();
                else
                    throw new NotSupportedException();
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(tasks);
            }
        }

        protected override void QueueTask(Task task)
        {
            lock (tasks)
            {
                tasks.AddLast(task);
                if (RunningTasks.ValueAsInt < MaxDeegreesOfParallelism)
                    NotifyThreadPoolOfPendingWork();
            }
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            bool result = true;
            if (!currentThreadIsProcessingTasks)
                result = false;
            if (taskWasPreviouslyQueued)
                TryDequeue(task);

            if (result)
                result &= TryExecuteTask(task);

            return result;
        }

        /// <summary>
        /// Attempts to remove a previously scheduled task from the scheduler
        /// </summary>
        /// <param name="task">The <see cref="Task"/> to dequeue</param>
        /// <returns><see langword="true"/> if the <paramref name="task"/> has been removed, <see langword="false"/> otherwise</returns>
        protected override sealed bool TryDequeue(Task task)
        {
            lock (tasks)
                return tasks.Remove(task);
        }

        #endregion TaskScheduler implementation

        #region Private methods

        /// <summary>
        /// Informs the ThreadPool that there is work to be executed by this scheduler.
        /// </summary>
        private void NotifyThreadPoolOfPendingWork()
        {
            ThreadPool.UnsafeQueueUserWorkItem((_) =>
                {
                    currentThreadIsProcessingTasks = true;
                    try
                    {
                        while (true)
                        {
                            Task nextTask;
                            lock (tasks)
                            {
                                if (tasks.Count == 0)
                                    break;

                                nextTask = tasks.First?.Value;
                                tasks.RemoveFirst();
                            }

                            ++RunningTasks.Value;
                            bool executed = TryExecuteTask(nextTask);

                            if (executed)
                                RunningTasks.Value = RunningTasks.Value > 0 ? RunningTasks.Value - 1 : 0d;
                        }
                    }
                    finally 
                    { 
                        currentThreadIsProcessingTasks = false; 
                    }
                },
                null
            );
        }

        #endregion Private methods
    }
}