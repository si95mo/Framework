using Core;
using Core.Conditions;
using Core.DataStructures;
using Core.Parameters;
using Diagnostic;
using Diagnostic.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    /// <summary>
    /// Implement generic aspects of the <see cref="IAwaitable"/> interface
    /// </summary>
    /// <remarks>
    /// <see cref="Awaitable"/> gives the possibility to have an <see cref="Alarm"/>, but the logic must be implemented.
    /// For a default firing logic implementation, consider using <see cref="AwaitableWithAlarm"/> instead
    /// (with both an <see cref="Alarm"/> and a <see cref="Warn"/> already implemented).
    /// Note that only an <see cref="Alarm"/> fired will trigger the <see cref="Fail(string)"/> method, not a <see cref="Warn"/>
    /// </remarks>
    public abstract class Awaitable : IAwaitable
    {
        private WaitForHandler waitForHandler;

        protected Alarm Alarm;
        protected Warn Warn;

        protected Scheduler Scheduler;
        protected bool StopRequested, FailRequested;

        #region Public fields

        public ICondition Running { get; private set; }
        public ICondition Completed { get; private set; }
        public EnumParameter<TaskStatus> Status { get; }
        public Parameter<Exception> Exception { get; protected set; }
        public CancellationTokenSource TokenSource { get; protected set; }
        public StringParameter WaitState { get; }

        public string Code { get; private set; }
        public object ValueAsObject { get => Status.Value; set => _ = value; }
        public Type Type => GetType();
        public Bag<IParameter> InputParameters { get; }
        public Bag<IParameter> OutputParameters { get; }

        #endregion Public fields

        /// <summary>
        /// Create a new instance of <see cref="Awaitable"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="scheduler">The <see cref="Scheduler"/> to use</param>
        protected Awaitable(string code, Scheduler scheduler = null)
        {
            Code = code;
            waitForHandler = new WaitForHandler();

            if (scheduler != null)
            {
                Scheduler = scheduler;
            }
            else
            {
                Scheduler = (Scheduler)ServiceBroker.GetService<SchedulersService>().DefaultScheduler;
            }

            Status = new EnumParameter<TaskStatus>($"{Code}.{nameof(Status)}", TaskStatus.Created);
            Exception = new ExceptionParameter(nameof(Exception));
            TokenSource = new CancellationTokenSource();
            WaitState = new StringParameter($"{Code}.{nameof(WaitState)}", string.Empty);

            Running = Status.IsEqualTo(TaskStatus.Running);
            Completed = Status.IsEqualTo(TaskStatus.RanToCompletion);

            InputParameters = new Bag<IParameter>();
            OutputParameters = new Bag<IParameter>();

            ServiceBroker.GetService<TasksService>().Add(this);
        }

        #region IAwaitable interface implementation

        public abstract IEnumerable<string> Execution();

        public virtual IEnumerable<string> Termination()
        {
            yield return "Done";
        }

        public virtual Task Start()
        {
            StopRequested = false;
            FailRequested = false;

            TokenSource.Dispose();
            TokenSource = new CancellationTokenSource();

            Status.Value = TaskStatus.WaitingToRun;

            Task task = new Task(() =>
                {
                    Status.Value = TaskStatus.Running;

                    try
                    {
                        // Iterate through the Execution task state
                        string oldWaitState = string.Empty;
                        IEnumerable<string> executionState = Execution();
                        foreach (string state in executionState)
                        {
                            // Exit loop if task has been canceled
                            if (TokenSource.Token.IsCancellationRequested)
                            {
                                if (FailRequested)
                                {
                                    Logger.Warn($"Task with code \"{Code}\" stop requested from {nameof(AlarmMonitor)}. The last wait state enumerated is \"{oldWaitState}\"");
                                }
                                else if (StopRequested)
                                {
                                    Logger.Info($"Task with code \"{Code}\" stop requested from outside. The last wait state enumerated is \"{oldWaitState}\"");
                                }

                                break;
                            }

                            WaitState.Value = state;
                            oldWaitState = state;
                        }

                        // And then through the termination task state (that shouldn't be canceled and will be executed)
                        IEnumerable terminationState = Termination();
                        foreach (string state in terminationState)
                        {
                            WaitState.Value = state;
                        }

                        Status.Value = TokenSource.Token.IsCancellationRequested ? TaskStatus.Canceled : TaskStatus.RanToCompletion;
                    }
                    catch (Exception ex) // This may be caused by a stop request or an actual exception
                    {
                        Status.Value = TaskStatus.Faulted;
                        Exception.Value = ex;

                        if (!FailRequested && !StopRequested)
                        {
                            Logger.Error($"Task with code \"{Code}\" faulted because of an internal exception. See below for more details");
                            Logger.Error(ex);
                        }
                        else if (StopRequested && !FailRequested)
                        {
                            Logger.Info($"Task with code \"{Code}\" faulted because a stop has been requested");
                        }
                        else if(FailRequested && !StopRequested)
                        {
                            Logger.Warn($"Task with code \"{Code}\" stop requested from {nameof(AlarmMonitor)}.");
                        }
                    }
                },
                TokenSource.Token
            );

            task.Start(Scheduler);
            return task;
        }

        public string Fail(string reasonOfFailure)
        {
            FailRequested = true;

            WaitState.Value = reasonOfFailure;
            Logger.Error($"Task with code \"{Code}\" failed, requesting stop. Reason of failure is \"{reasonOfFailure}\"");

            OnFail();
            Stop();

            Status.Value = TaskStatus.Faulted;

            return reasonOfFailure;
        }

        public TaskAwaiter GetAwaiter()
        {
            TaskAwaiter awaiter = Start().GetAwaiter();
            return awaiter;
        }

        public virtual void Stop()
        {
            if (Status.Value == TaskStatus.Running || Status.Value == TaskStatus.WaitingToRun)
            {
                StopRequested = true;
                TokenSource.Cancel();

                Status.Value = TaskStatus.Canceled;
            }
        }

        public virtual void Stop(TimeSpan delay)
        {
            if (Status.Value == TaskStatus.Running || Status.Value == TaskStatus.WaitingToRun)
            {
                StopRequested = true;
                TokenSource.CancelAfter(delay);

                Status.Value = TaskStatus.Canceled;
            }
        }

        public virtual void Stop(int delay)
            => Stop(TimeSpan.FromMilliseconds(delay));

        #endregion IAwaitable interface implementation

        #region Protected methods

        #region WaitForHandler

        /// <summary>
        /// Awaits a fixed time
        /// </summary>
        /// <param name="timeToWait">The time to wait</param>
        /// <returns>The corresponding <see cref="WaitForHandler"/></returns>
        protected WaitForHandler WaitFor(TimeSpan timeToWait)
            => waitForHandler.Await(timeToWait);

        /// <summary>
        /// Awaits a .NET <see cref="Task"/>
        /// </summary>
        /// <param name="task">The <see cref="Task"/> to wait</param>
        /// <returns>The corresponding <see cref="WaitForHandler"/></returns>
        protected WaitForHandler WaitFor(Task task)
            => waitForHandler.Await(task);

        /// <summary>
        /// Awaits a collection of .NET <see cref="Task"/>
        /// </summary>
        /// <param name="tasks">The collection of <see cref="Task"/> to wait</param>
        /// <returns>The corresponding <see cref="WaitForHandler"/></returns>
        protected WaitForHandler WaitFor(Task[] tasks)
            => waitForHandler.Await(tasks);

        /// <summary>
        /// Awaits a .NET <see cref="Task"/>
        /// </summary>
        /// <param name="task">The <see cref="Task"/> to wait</param>
        /// <param name="result">The result of <paramref name="task"/> execution</param>
        /// <returns>The corresponding <see cref="WaitForHandler"/></returns>
        protected WaitForHandler WaitFor<T>(Task<T> task, out T result)
            => waitForHandler.Await(task, out result);

        /// <summary>
        /// Awaits an <see cref="IAwaitable"/> task
        /// </summary>
        /// <param name="task">The <see cref="IAwaitable"/> to wait</param>
        /// <returns>The corresponding <see cref="WaitForHandler"/></returns>
        protected WaitForHandler WaitFor(IAwaitable task)
            => waitForHandler.Await(task);

        /// <summary>
        /// Awaits a collection of <see cref="IAwaitable"/> tasks
        /// </summary>
        /// <param name="tasks">The collection of <see cref="IAwaitable"/> to wait</param>
        /// <returns>The corresponding <see cref="WaitForHandler"/></returns>
        protected WaitForHandler WaitFor(IAwaitable[] tasks)
            => waitForHandler.Await(tasks);

        /// <summary>
        /// Wait for an <see cref="ICondition"/> to be <see langword="true"/>
        /// </summary>
        /// <param name="condition">The condition to wait</param>
        /// <returns>The corresponding <see cref="WaitForHandler"/></returns>
        protected WaitForHandler WaitFor(ICondition condition)
            => waitForHandler.Await(condition);

        /// <summary>
        /// Wait for an <see cref="ICondition"/> to be <see langword="true"/>, with a timeout
        /// </summary>
        /// <param name="condition">The condition to wait</param>
        /// <param name="timeoutInMilliseconds">The timeout in milliseconds</param>
        /// <returns>The corresponding <see cref="WaitForHandler"/></returns>
        protected WaitForHandler WaitFor(ICondition condition, int timeoutInMilliseconds)
            => waitForHandler.Await(condition, timeoutInMilliseconds);

        /// <summary>
        /// Wait for an <see cref="ICondition"/> to be <see langword="true"/>, with a timeout
        /// </summary>
        /// <param name="condition">The condition to wait</param>
        /// <param name="timeout">The timeout <see cref="TimeSpan"/></param>
        /// <returns>The corresponding <see cref="WaitForHandler"/></returns>
        protected WaitForHandler WaitFor(ICondition condition, TimeSpan timeout)
            => waitForHandler.Await(condition, timeout);

        /// <summary>
        /// Await for a subroutine, as a collection of <see cref="string"/>
        /// </summary>
        /// <param name="subroutine">The subroutine to invoke and await</param>
        /// <returns>The corresponding <see cref="WaitForHandler"/></returns>
        protected WaitForHandler WaitFor(IEnumerable<string> subRoutine)
            => waitForHandler.Await(subRoutine);

        #endregion WaitForHandler

        /// <summary>
        /// Called just before the <see cref="IAwaitable"/> task stop in case of <see cref="Fail(string)"/>
        /// </summary>
        protected virtual void OnFail()
        { }

        #endregion Protected methods

        #region Private methods

        private void AlarmFired_ValueChanged(object sender, ValueChangedEventArgs e)
            => Fail("Alarm fired");

        #endregion Private methods
    }

    /// <summary>
    /// Define an <see cref="Exception"/> <see cref="Parameter{T}"/>
    /// </summary>
    public class ExceptionParameter : Parameter<Exception>
    {
        /// <summary>
        /// Create a new instance of <see cref="ExceptionParameter"/>
        /// </summary>
        /// <param name="code">The code</param>
        public ExceptionParameter(string code) : base(code)
        { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder($"Exception message: {Value?.Message}{Environment.NewLine}");
            sb.Append($"\t\tStack-trace: {Value?.StackTrace}{Environment.NewLine}");

            return sb.ToString();
        }
    }
}