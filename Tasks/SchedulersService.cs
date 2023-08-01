using Core.DataStructures;

namespace Tasks
{
    /// <summary>
    /// Define a service for <see cref="IScheduler"/>
    /// </summary>
    public class SchedulersService : Service<IScheduler>
    {
        /// <summary>
        /// The default <see cref="IScheduler"/>
        /// </summary>
        public IScheduler DefaultScheduler { get; private set; }

        /// <summary>
        /// Create a new instance of <see cref="SchedulersService"/>
        /// </summary>
        /// <param name="maxDegreesOfParallelism">The maximum degrees of parallelism of the <see cref="DefaultScheduler"/></param>
        public SchedulersService(int maxDegreesOfParallelism = 100) : base()
        {
            CreateDefaultScheduler(maxDegreesOfParallelism);
        }

        /// <summary>
        /// Create a new instance of <see cref="SchedulersService"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="maxDegreesOfParallelism">The maximum degrees of parallelism of the <see cref="DefaultScheduler"/></param>
        public SchedulersService(string code, int maxDegreesOfParallelism = 100) : base(code)
        {
            CreateDefaultScheduler(maxDegreesOfParallelism);
        }

        /// <summary>
        /// Create the <see cref="DefaultScheduler"/>
        /// </summary>
        /// <param name="maxDegreesOfParallelism">The maximum degrees of parallelism of the <see cref="DefaultScheduler"/></param>
        private void CreateDefaultScheduler(int maxDegreesOfParallelism)
        {
            DefaultScheduler = new Scheduler("DefaultScheduler", maxDegreesOfParallelism);
            Add(DefaultScheduler);
        }
    }
}