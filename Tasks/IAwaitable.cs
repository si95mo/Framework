using Core;
using Core.DataStructures;
using Core.Parameters;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    /// <summary>
    /// Define a generic awaitable task
    /// </summary>
    public interface IAwaitable : IProperty
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
        StringParameter WaitState { get; }

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
        /// <returns>The <see cref="IEnumerable{T}"/> execution state associated with the <see cref="IAwaitable"/></returns>
        IEnumerable<string> Execution();

        /// <summary>
        /// The body of the <see cref="IAwaitable"/> termination.
        /// This is the method that is called when <see cref="Start"/> is invoked and <see cref="Execution"/> end
        /// </summary>
        /// <returns>The <see cref="IEnumerable{T}"/> execution state associated with the <see cref="IAwaitable"/> termination</returns>
        IEnumerable<string> Termination();

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

        /// <summary>
        /// Get the <see cref="TaskAwaiter"/> to use the <see langword="await"/> keyword
        /// </summary>
        /// <returns>The <see cref="TaskAwaiter"/></returns>
        TaskAwaiter GetAwaiter();

        /// <summary>
        /// Let the <see cref="IAwaitable"/> task fail
        /// </summary>
        /// <param name="reasonOfFailure">The reason of failure</param>
        void Fail(string reasonOfFailure);
    }
}