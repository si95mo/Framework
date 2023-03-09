﻿using DiagnosticMessages;
using Core;
using Core.DataStructures;
using Core.Parameters;
using Diagnostic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    public abstract class Awaitable : IAwaitable, IProperty
    {
        private WaitForHandler waitForHandler;
        private bool stopRequested;

        protected Alarm Alarm;
        protected Warn Warn;

        #region Public fields

        public EnumParameter<TaskStatus> Status { get; }
        public CancellationTokenSource TokenSource { get; }
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
        protected Awaitable(string code)
        {
            Code = code;
            waitForHandler = new WaitForHandler();

            Status = new EnumParameter<TaskStatus>($"{Code}.{nameof(Status)}", TaskStatus.Created);
            TokenSource = new CancellationTokenSource();
            WaitState = new StringParameter($"{Code}.{nameof(WaitState)}", string.Empty);

            InputParameters = new Bag<IParameter>();
            OutputParameters = new Bag<IParameter>();
        }

        #region IAwaitable interface implementation

        public abstract IEnumerable<string> Execution();

        public virtual IEnumerable<string> Termination()
        {
            yield return "Done";
        }

        public Task Start()
        {
            stopRequested = false;
            Status.Value = TaskStatus.WaitingToRun;

            Task task = new Task(() =>
                {
                    Status.Value = TaskStatus.Running;

                    try
                    {
                        // Iterate through the Execution task state
                        IEnumerable<string> executionState = Execution();
                        foreach (string state in executionState)
                            WaitState.Value = state;

                        // And then through the termination task state
                        IEnumerable terminationState = Termination();
                        foreach (string state in terminationState)
                            WaitState.Value = state;

                        Status.Value = TaskStatus.RanToCompletion;
                    }
                    catch (Exception ex) // This may be cuased by a stop request or an actual exception
                    {
                        Status.Value = TaskStatus.Faulted;

                        if (stopRequested)
                            Logger.Error(ex);
                        else
                            Logger.Warn($"Task with code {Code} faulted because a stop has been requested");
                    }
                },
                TokenSource.Token
            );

            task.Start();
            return task;
        }

        public void Fail(string reasonOfFailure)
        {
            WaitState.Value = reasonOfFailure;
            Logger.Error($"Task with code {Code} failed, requesting stop. {reasonOfFailure}");

            OnFail();
            Stop();

            Status.Value = TaskStatus.Faulted;
        }

        public TaskAwaiter GetAwaiter()
        {
            TaskAwaiter awaiter = Start().GetAwaiter();
            return awaiter;
        }

        public virtual void Stop()
        {
            stopRequested = true;

            TokenSource.Cancel();
            TokenSource.Token.ThrowIfCancellationRequested(); // Let the task fail and then stop

            Status.Value = TaskStatus.Canceled;
        }

        public virtual void Stop(TimeSpan delay)
        {
            stopRequested = true;

            TokenSource.CancelAfter(delay);
            TokenSource.Token.ThrowIfCancellationRequested(); // Let the task fail and then stop

            Status.Value = TaskStatus.Canceled;
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
        /// Awaits an <see cref="IAwaitable"/> task
        /// </summary>
        /// <param name="task">The <see cref="IAwaitable"/> to wait</param>
        /// <returns>The corresponding <see cref="WaitForHandler"/></returns>
        protected WaitForHandler WaitFor(IAwaitable task)
            => waitForHandler.Await(task);

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
}