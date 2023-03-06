using Alarms;
using Core;
using Core.DataStructures;
using Core.Parameters;
using Devices;
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
    public abstract class Awaitable : IAwaitable, IProperty
    {
        private string code;
        private WaitForHandler waitForHandler;

        protected Alarm Alarm;

        public EnumParameter<TaskStatus> Status { get; }
        public CancellationTokenSource TokenSource { get; }
        public StringParameter WaitState { get; }

        public string Code => code;
        public object ValueAsObject { get => Status.Value; set => _ = value; }
        public Type Type => GetType();
        public Bag<IParameter> InputParameters { get; }
        public Bag<IParameter> OutputParameters { get; }

        /// <summary>
        /// Create a new instance of <see cref="Awaitable"/>
        /// </summary>
        /// <param name="code">The code</param>
        protected Awaitable(string code)
        {
            this.code = code;
            waitForHandler = new WaitForHandler();

            Status = new EnumParameter<TaskStatus>($"{Code}.{nameof(Status)}", TaskStatus.Created);
            TokenSource = new CancellationTokenSource();
            WaitState = new StringParameter($"{Code}.{nameof(WaitState)}", string.Empty);

            InputParameters = new Bag<IParameter>();
            OutputParameters = new Bag<IParameter>();
        }

        private void AlarmFired_ValueChanged(object sender, ValueChangedEventArgs e)
            => Fail("Alarm fired");

        #region IAwaitable interface implementation

        public abstract IEnumerable<string> Execution();

        public virtual IEnumerable<string> Termination()
        {
            yield return "Done";
        }

        public Task Start()
        {
            Status.Value = TaskStatus.WaitingToRun;

            Task task = new Task(() =>
                {
                    Status.Value = TaskStatus.Running;

                    try
                    {
                        IEnumerable<string> executionState = Execution();
                        foreach (string state in executionState)
                            WaitState.Value = state;

                        IEnumerable terminationState = Termination();
                        foreach (string state in terminationState)
                            WaitState.Value = state;

                        Status.Value = TaskStatus.RanToCompletion;
                    }
                    catch (Exception ex)
                    {
                        Status.Value = TaskStatus.Faulted;
                        Logger.Error(ex);
                    }
                },
                TokenSource.Token
            );

            task.Start();
            return task;
        }

        public TaskAwaiter GetAwaiter()
        {
            TaskAwaiter awaiter = Start().GetAwaiter();
            return awaiter;
        }

        public virtual void Stop()
        {
            TokenSource.Cancel();
            TokenSource.Token.ThrowIfCancellationRequested();

            Status.Value = TaskStatus.Canceled;
        }

        public virtual void Stop(TimeSpan delay)
        {
            TokenSource.CancelAfter(delay);
            TokenSource.Token.ThrowIfCancellationRequested();

            Status.Value = TaskStatus.Canceled;
        }

        public virtual void Stop(int delay)
            => Stop(TimeSpan.FromMilliseconds(delay));

        /// <summary>
        /// Let the <see cref="Awaitable"/> fail
        /// </summary>
        /// <param name="reasonOfFailure"></param>
        public void Fail(string reasonOfFailure)
        {
            WaitState.Value = reasonOfFailure;

            OnFail();
            Stop();

            Status.Value = TaskStatus.Faulted;
        }

        #endregion IAwaitable interface implementation

        #region Protected methods

        protected WaitForHandler WaitFor(TimeSpan timeToWait) 
            => waitForHandler.Await(timeToWait);

        protected WaitForHandler WaitFor(Task task)
            => waitForHandler.Await(task);

        /// <summary>
        /// Called just before the <see cref="IAwaitable"/> task stop in case of <see cref="Fail(string)"/>
        /// </summary>
        protected virtual void OnFail() 
        { }

        #endregion Protected methods
    }
}