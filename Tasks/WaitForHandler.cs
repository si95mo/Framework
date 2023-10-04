using Core;
using Extensions;
using Nito.AsyncEx;
using System;
using System.Threading.Tasks;

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

        private Action actionWhenDone, actionInTheMeanwhile;

        /// <summary>
        /// The <see cref="WaitForHandler"/> message
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        /// Create a new instance of <see cref="WaitForHandler"/>
        /// </summary>
        public WaitForHandler()
        {
            Code = Guid.NewGuid().ToString(); // Just assign a unique code
        }

        /// <summary>
        /// Await a specified <see cref="TimeSpan"/>
        /// </summary>
        /// <param name="timeToWait">The tome to wait</param>
        /// <returns>The <see cref="WaitForHandler"/></returns>
        public WaitForHandler Await(TimeSpan timeToWait)
        {
            AsyncContext.Run(async () => await this.WaitFor(timeToWait: timeToWait));
            Message = $"Waiting {timeToWait}";

            return this;
        }

        /// <summary>
        /// Await a .NET <see cref="Task"/>
        /// </summary>
        /// <param name="task">The <see cref="Task"/> to wait</param>
        /// <returns>The <see cref="WaitForHandler"/></returns>
        public WaitForHandler Await(Task task)
        {
            AsyncContext.Run(async () => await this.WaitFor(task));
            Message = "Waiting a .NET task";

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
            AsyncContext.Run(async () => await task);

            task.WaitState.ValueChanged -= WaitState_ValueChanged;

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

        public WaitForHandler WhenDone(Action action)
        {
            actionWhenDone = action;
            return this;
        }

        public WaitForHandler InTheMeanwhile(Action action)
        {
            actionInTheMeanwhile = action;
            return this;
        }

        /// <summary>
        /// Implicitly convert an <see cref="WaitForHandler"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="waitForHandler">The <see cref="WaitForHandler"/></param>
        /// <returns>The conversion result</returns>
        public static implicit operator string(WaitForHandler waitForHandler)
            => waitForHandler.Message;
    }
}