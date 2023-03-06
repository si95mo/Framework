using Extensions;
using Nito.AsyncEx;
using System;
using System.Threading.Tasks;

namespace Tasks
{
    /// <summary>
    /// Define an hadler to wait for generic tasks
    /// </summary>
    public class WaitForHandler
    {
        /// <summary>
        /// The <see cref="WaitForHandler"/> message
        /// </summary>
        public string Message { get; protected set; }

        public WaitForHandler() 
        { }


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

        private void WaitState_ValueChanged(object sender, Core.ValueChangedEventArgs e)
            => Message = e.NewValueAsString;

        /// <summary>
        /// Add a custom description as the <see cref="WaitForHandler.Message"/>
        /// </summary>
        /// <param name="description">The custom description to set</param>
        /// <returns>The <see cref="WaitForHandler"/></returns>
        public WaitForHandler WithDescription(string description)
        {
            Message = description;
            return this;
        }

        /// <summary>
        /// Implicitally convert an <see cref="WaitForHandler"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="waitForHandler">The <see cref="WaitForHandler"/></param>
        /// <returns>The conversion result</returns>
        public static implicit operator string(WaitForHandler waitForHandler)
            => waitForHandler.Message; 
    }
}
