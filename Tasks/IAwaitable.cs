using Core.DataStructures;
using Core.Parameters;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    /// <summary>
    /// Define a generic awaitable task
    /// </summary>
    public interface IAwaitable
    {
        /// <summary>
        /// The <see cref="IAwaitable"/> <see cref="TaskStatus"/>
        /// </summary>
        EnumParameter<TaskStatus> Status { get; }

        /// <summary>
        /// The <see cref="IAwaitable"/> task <see cref="CancellationTokenSource"/>
        /// </summary>
        CancellationTokenSource TokenSource { get; }

        /// <summary>
        /// The actual <see cref="IAwaitable"/> message (i.e. last executed operation)
        /// </summary>
        StringParameter Message { get; }

        /// <summary>
        /// The <see cref="IAwaitable"/> input <see cref="IParameter"/>
        /// </summary>
        Bag<IParameter> InputParameters { get; }

        /// <summary>
        /// The <see cref="IAwaitable"/> output <see cref="IParameter"/>
        /// </summary>
        Bag<IParameter> OutputParameters { get; }

        /// <summary>
        /// The body of the <see cref="IAwaitable"/> task.
        /// This is the method that is called when <see cref="Start"/> is invoked
        /// </summary>
        /// <returns>The actual <see cref="Task"/> body associated with the <see cref="IAwaitable"/></returns>
        Task Execute();

        /// <summary>
        /// Start the <see cref="IAwaitable"/> task
        /// </summary>
        /// <returns>The actual <see cref="Task"/> associated with the <see cref="IAwaitable"/></returns>
        Task Start();

        /// <summary>
        /// Stop the <see cref="IAwaitable"/> task
        /// </summary>
        void Stop();

        /// <summary>
        /// Stop the <see cref="IAwaitable"/> task after an amount of time
        /// </summary>
        /// <param name="delay">The time to wait for the task to stop</param>
        void Stop(TimeSpan delay);

        /// <summary>
        /// Stop the <see cref="IAwaitable"/> task after an amount of time
        /// </summary>
        /// <param name="delay">The time to wait for the task to stop (in milliseconds)</param>
        void Stop(int delay);
    }
}