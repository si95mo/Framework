using Core.DataStructures;
using System.Linq;

namespace Tasks
{
    /// <summary>
    /// Define a service for <see cref="IScheduler"/>
    /// </summary>
    public class SchedulersService : Service<IScheduler>
    {
        /// <summary>
        /// Create a new instance of <see cref="SchedulersService"/>
        /// </summary>
        public SchedulersService() : base()
        { }

        /// <summary>
        /// Create a new instance of <see cref="SchedulersService"/>
        /// </summary>
        /// <param name="code">The code</param>
        public SchedulersService(string code) : base(code)
        { }

        /// <summary>
        /// Get the default <see cref="IScheduler"/>
        /// </summary>
        /// <returns>The default <see cref="IScheduler"/></returns>
        public IScheduler GetDefaultScheduler()
        {
            IScheduler scheduler;
            if (Subscribers.Count == 0) // If there are no subscribers
            {
                scheduler = new Scheduler(); // Create a new scheduler
                Add(scheduler); // And add it
            }
            else
                scheduler = (IScheduler)GetAll().ElementAt(0); // Else get the first scheduler that has subscribed

            return scheduler;
        }
    }
}