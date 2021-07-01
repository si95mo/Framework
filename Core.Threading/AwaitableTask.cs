using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Core.Threading
{
    public class AwaitableTask : IAwaitable
    {
        private string code;
        private bool running, succeded, notSucceded;
        private Failure failure;
        private TaskStatus status;
        private TaskResult result;
        private Task executionTask;

        public string Code => code;

        public TaskStatus Status => status;

        public TaskResult Result => result;

        public bool Running => running;

        public bool Succeded => succeded;

        public bool NotSucceded => notSucceded;

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