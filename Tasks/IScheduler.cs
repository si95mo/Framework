using Core;
using Core.Parameters;

namespace Tasks
{
    public interface IScheduler : IProperty
    {
        /// <summary>
        /// The number of running <see cref="Awaitable"/> tasks
        /// </summary>
        NumericParameter RunningTasks { get; }

        /// <summary>
        /// The actual <see cref="IScheduler"/> load
        /// </summary>
        NumericParameter Load { get; }
    }
}