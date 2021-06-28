using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Core.Threading
{
    /// <summary>
    /// Defines the <see cref="IAwaitable"/>
    /// task status
    /// </summary>
    public enum TaskResult
    {
        /// <summary>
        /// None, default
        /// </summary>
        None = 0,
        /// <summary>
        /// The <see cref="IAwaitable"/> task succeeded
        /// </summary>
        Success = 1,
        /// <summary>
        /// The <see cref="IAwaitable"/> task 
        /// was aborted
        /// </summary>
        Aborted = 2,
        /// <summary>
        /// The <see cref="IAwaitable"/> task failed
        /// </summary>
        Failure = 3
    }

    /// <summary>
    /// Describe an awaitable task
    /// </summary>
    public interface IAwaitable
    {
        /// <summary>
        /// The <see cref="IAwaitable"/> code
        /// </summary>
        string Code { get; }

        /// <summary>
        /// The <see cref="IAwaitable"/> <see cref="TaskStatus"/>
        /// </summary>
        TaskStatus Status { get; }

        /// <summary>
        /// The <see cref="IAwaitable"/> <see cref="TaskResult"/>
        /// </summary>
        TaskResult Result { get; }

        /// <summary>
        /// Determine whether the <see cref="IAwaitable"/>
        /// task is running or not
        /// </summary>
        bool Running { get; }

        /// <summary>
        /// Determine whether the <see cref="IAwaitable"/>
        /// task succeeded. See also <see cref="NotSucceded"/>
        /// </summary>
        bool Succeded { get; }

        /// <summary>
        /// Determine whether the <see cref="IAwaitable"/>
        /// task not succeeded. See also <see cref="Succeded"/>
        /// </summary>
        bool NotSucceded { get; }

        /// <summary>
        /// The <see cref="IAwaitable"/> task <see cref="IFailure"/>
        /// </summary>
        IFailure Failure { get; }

        /// <summary>
        /// Get the <see cref="IAwaitable"/> task 
        /// awaiter. See also <see cref="TaskResult"/>
        /// </summary>
        /// <returns>The <see cref="TaskAwaiter"/> of the 
        /// <see cref="IAwaitable"/> task</returns>
        TaskAwaiter<TaskResult> GetAwaiter();

        /// <summary>
        /// Execute the awaitable task
        /// </summary>
        /// <returns>The <see cref="IAwaitable"/> task </returns>
        IAwaitable Execute();
    }
}
