using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Core.Threading
{
    /// <summary>
    /// Define an awaitable task
    /// </summary>
    public class AwaitableTask : IAwaitable
    {
        private string code;
        private bool running, succeded, notSucceded;
        private Failure failure;
        private TaskStatus status;
        private TaskResult result;
        private Task executionTask;

        /// <summary>
        /// The code
        /// </summary>
        public string Code => code;

        /// <summary>
        /// The <see cref="TaskStatus"/>
        /// </summary>
        public TaskStatus Status => status;

        /// <summary>
        /// The <see cref="TaskResult"/>
        /// </summary>
        public TaskResult Result => result;

        /// <summary>
        /// The running state
        /// </summary>
        public bool Running => running;

        /// <summary>
        /// The succeeded state
        /// </summary>
        public bool Succeded => succeded;

        /// <summary>
        /// The not succeded state
        /// </summary>
        public bool NotSucceded => notSucceded;

        /// <summary>
        /// The <see cref="IFailure"/>
        /// </summary>
        public IFailure Failure => failure;

        /// <summary>
        /// Create a new instance of <see cref="AwaitableTask"/>
        /// with default parameters
        /// </summary>
        public AwaitableTask()
        {
            code = Guid.NewGuid().ToString();

            running = false;
            succeded = false;
            notSucceded = false;

            status = TaskStatus.WaitingForActivation;
            result = TaskResult.None;

            executionTask = new Task(
                () => Execute()
            );
        }

        /// <summary>
        /// Create a new instance of <see cref="AwaitableTask"/>
        /// </summary>
        /// <param name="code">The code</param>
        public AwaitableTask(string code) : this()
        {
            this.code = code;
        }

        /// <summary>
        /// Action to be executed
        /// </summary>
        /// <returns>The <see cref="IAwaitable"/> task</returns>
        public IAwaitable Execute()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the <see cref="AwaitableTask"/> awaiter
        /// </summary>
        /// <returns>The <see cref="TaskAwaiter{TResult}"/></returns>
        public TaskAwaiter<TaskResult> GetAwaiter()
        {
            TaskAwaiter<TaskResult> awaiter = new TaskAwaiter<TaskResult>();

            return awaiter;
        }
    }
}