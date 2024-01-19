using Core;
using Core.DataStructures;
using Core.Parameters;

namespace Tasks
{
    public interface IScheduler : IProperty
    {
        /// <summary>
        /// The maximum concurrency level allowed by this scheduler.
        /// </summary>
        int MaxDeegreesOfParallelism { get; }

        /// <summary>
        /// The number of running <see cref="Awaitable"/> tasks
        /// </summary>
        NumericParameter RunningTasks { get; }

        /// <summary>
        /// The actual <see cref="IScheduler"/> load
        /// </summary>
        NumericParameter Load { get; }

        /// <summary>
        /// The <see cref="Bag{T}"/> with all the <see cref="IScheduler"/> associated <see cref="IAwaitable"/> tasks
        /// </summary>
        Bag<IAwaitable> Tasks { get; }
    }
}