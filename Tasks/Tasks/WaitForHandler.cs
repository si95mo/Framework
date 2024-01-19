using Core;
using Core.Conditions;
using Extensions;
using Nito.AsyncEx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml;

namespace Tasks
{
    /// <summary>
    /// Define an handler to wait for generic tasks
    /// </summary>
    public class WaitForHandler : IProperty
    {
        #region IProperty (fake) implementation

        public string Code { get; private set; }
        public object ValueAsObject { get; set; }
        public Type Type => GetType();

        #endregion IProperty (fake) implementation

        /// <summary>
        /// The <see cref="WaitForHandler"/> message
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        /// Check if the <see cref="WaitForHandler"/> has an execution timeout
        /// </summary>
        public bool HasTimeout => timeout != TimeSpan.Zero;

        /// <summary>
        /// Define the completed status (<see langword="true"/>) or faulted/timeout/error case
        /// </summary>
        public bool Completed { get; private set; }

        private TimeSpan timeout;

        /// <summary>
        /// Create a new instance of <see cref="WaitForHandler"/>
        /// </summary>
        public WaitForHandler()
        {
            Code = Guid.NewGuid().ToString(); // Just assign a unique code
            timeout = TimeSpan.Zero;
        }

        /// <summary>
        /// Await a specified <see cref="TimeSpan"/>
        /// </summary>
        /// <param name="timeToWait">The tome to wait</param>
        /// <returns>The <see cref="WaitForHandler"/></returns>
        public WaitForHandler Await(TimeSpan timeToWait)
        {
            ResetTimeout();

            AsyncContext.Run(async () => await this.WaitFor(timeToWait: timeToWait));
            Message = $"Waiting {timeToWait}";

            Completed = true;

            return this;
        }

        /// <summary>
        /// Await a .NET <see cref="Task"/>
        /// </summary>
        /// <param name="task">The <see cref="Task"/> to wait</param>
        /// <returns>The <see cref="WaitForHandler"/></returns>
        public WaitForHandler Await(Task task)
        {
            Completed = AsyncContext.Run(async () => await this.WaitFor(task, timeout));
            Message = "Waiting a .NET task";

            ResetTimeout();

            return this;
        }

        /// <summary>
        /// Await a collection of .NET <see cref="Task"/>
        /// </summary>
        /// <param name="task">The <see cref="Task"/> collection to wait</param>
        /// <returns>The <see cref="WaitForHandler"/></returns>
        public WaitForHandler Await(Task[] tasks)
        {
            foreach (Task task in tasks)
            {
                Await(task);
            }
            return this;
        }

        /// <summary>
        /// Await a .NET <see cref="Task{TResult}"/>
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Task{TResult}"/> to await</typeparam>
        /// <param name="task">The <see cref="Task{TResult}"/> to wait</param>
        /// <param name="result">The <paramref name="task"/> execution result</param>
        /// <returns>The <see cref="WaitForHandler"/></returns>
        public WaitForHandler Await<T>(Task<T> task, out T result)
        {
            T localResult = default;

            if (HasTimeout)
            {
                Task waitTask = Task.Delay(timeout);
                Completed = AsyncContext.Run(async () => (await Task.WhenAny(new Task(async () => localResult = await this.WaitFor(task)), waitTask)) != waitTask);
            }
            else
            {
                localResult = AsyncContext.Run(async () => await this.WaitFor(task));
                Completed = true;
            }

            result = localResult;

            ResetTimeout();

            return this;
        }

        /// <summary>
        /// Await an <see cref="IAwaitable"/> task
        /// </summary>
        /// <param name="task">The <see cref="IAwaitable"/> to wait</param>
        /// <returns>The <see cref="WaitForHandler"/></returns>
        public WaitForHandler Await(IAwaitable task)
        {
            task.WaitState.ValueChanged += WaitState_ValueChanged;

            if (timeout > TimeSpan.Zero)
            {
                Task waitTask = Task.Delay(timeout);
                Completed = AsyncContext.Run(async () => await Task.WhenAny(new Task(async () => await task), waitTask) != waitTask);
            }
            else
            {
                AsyncContext.Run(async () => await task);
                Completed = true;
            }

            ResetTimeout();

            task.WaitState.ValueChanged -= WaitState_ValueChanged;

            return this;
        }

        /// <summary>
        /// Await a collection of <see cref="IAwaitable"/> tasks
        /// </summary>
        /// <param name="task">The <see cref="IAwaitable"/> collection to wait</param>
        /// <returns>The <see cref="WaitForHandler"/></returns>
        public WaitForHandler Await(IAwaitable[] tasks)
        {
            foreach (IAwaitable task in tasks)
            {
                Await(task);
            }
            return this;
        }

        /// <summary>
        /// Await an <see cref="ICondition"/> to be <see langword="true"/>
        /// </summary>
        /// <param name="condition">The <see cref="ICondition"/> to wait</param>
        /// <returns>The <see cref="WaitForHandler"/></returns>
        public WaitForHandler Await(ICondition condition)
        {
            if (timeout > TimeSpan.Zero)
            {
                Completed = AsyncContext.Run(async () => await this.WaitFor(condition, (int)timeout.TotalMilliseconds));
            }
            else
            {
                AsyncContext.Run(async () => await this.WaitFor(condition));
                Completed = true;
            }

            ResetTimeout();
            return this;
        }

        /// <summary>
        /// Await an <see cref="ICondition"/> to be <see langword="true"/> with a timeout
        /// </summary>
        /// <param name="condition">The <see cref="ICondition"/> to wait</param>
        /// <param name="timeoutInMilliseconds">The timeout in milliseconds</param>
        /// <returns>The <see cref="WaitForHandler"/><returns>
        public WaitForHandler Await(ICondition condition, int timeoutInMilliseconds)
        {
            AsyncContext.Run(async () => await this.WaitFor(condition, timeoutInMilliseconds));
            return this;
        }

        /// <summary>
        /// Await an <see cref="ICondition"/> to be <see langword="true"/> with a timeout
        /// </summary>
        /// <param name="condition">The <see cref="ICondition"/> to wait</param>
        /// <param name="timeout">The timeout <see cref="TimeSpan"/></param>
        /// <returns>The <see cref="WaitForHandler"/><returns>
        public WaitForHandler Await(ICondition condition, TimeSpan timeout)
            => Await(condition, (int)timeout.TotalMilliseconds);

        /// <summary>
        /// Await for a subroutine, as a collection of <see cref="string"/>
        /// </summary>
        /// <param name="subroutine">The subroutine to invoke and await</param>
        /// <returns>The <see cref="WaitForHandler"/></returns>
        public WaitForHandler Await(IEnumerable<string> subroutine)
        {
            Stopwatch timer = Stopwatch.StartNew();
            foreach (string instruction in subroutine)
            {
                if (timeout > TimeSpan.Zero && timer.Elapsed <= timeout)
                {
                    Message = instruction;
                }
                else if (timeout > TimeSpan.Zero && timer.Elapsed > timeout)
                {
                    Completed = false;
                    return this;
                }
            }

            Completed = true;
            return this;
        }

        private void WaitState_ValueChanged(object sender, ValueChangedEventArgs e)
            => Message = e.NewValueAsString;

        /// <summary>
        /// Add a custom description message as the <see cref="Message"/>
        /// </summary>
        /// <param name="message7">The custom description message to set</param>
        /// <returns>The <see cref="WaitForHandler"/></returns>
        public WaitForHandler WithMessage(string message)
        {
            Message = message;
            return this;
        }

        /// <summary>
        /// Add a timeout to the <see cref="WaitForHandler"/> execution
        /// </summary>
        /// <param name="timeout">The timeout <see cref="TimeSpan"/></param>
        /// <returns>The <see cref="WaitForHandler"/></returns>
        public WaitForHandler WithTimeout(TimeSpan timeout)
        {
            this.timeout = timeout;
            return this;
        }

        /// <summary>
        /// Add a timeout to the <see cref="WaitForHandler"/> execution
        /// </summary>
        /// <param name="timeoutInSeconds">The timeout in seconds</param>
        /// <returns>The <see cref="WaitForHandler"/></returns>
        public WaitForHandler WithTimeout(double timeoutInSeconds)
            => WithTimeout(TimeSpan.FromSeconds(timeoutInSeconds));

        /// <summary>
        /// Remove the timeout to the <see cref="WaitForHandler"/> execution
        /// </summary>
        /// <returns>The <see cref="WaitForHandler"/></returns>
        public WaitForHandler WithoutTimeout()
        {
            timeout = TimeSpan.Zero;
            return this;
        }

        /// <summary>
        /// Implicitly convert an <see cref="WaitForHandler"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="waitForHandler">The <see cref="WaitForHandler"/></param>
        /// <returns>The conversion result</returns>
        public static implicit operator string(WaitForHandler waitForHandler)
            => waitForHandler.Message;

        /// <summary>
        /// Reset the internal timeout
        /// </summary>
        private void ResetTimeout()
        {
            timeout = TimeSpan.Zero;
        }
    }
}