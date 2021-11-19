using Core;
using Core.DataStructures;
using Core.Parameters;
using Diagnostic;
using System;
using System.Threading.Tasks;

namespace Hardware
{
    /// <summary>
    /// Describe a generic hardware resource. <br/>
    /// See also <see cref="IResource"/>
    /// </summary>
    public abstract class Resource : IResource
    {
        /// <summary>
        /// The code
        /// </summary>
        protected string code;

        /// <summary>
        /// The status
        /// </summary>
        protected EnumParameter<ResourceStatus> status;

        /// <summary>
        /// The failure
        /// </summary>
        protected IFailure failure;

        /// <summary>
        /// The channels
        /// </summary>
        protected Bag<IChannel> channels;

        private object objectLock = new object();

        /// <summary>
        /// Create a new instance of <see cref="Resource"/>
        /// </summary>
        /// <param name="code"></param>
        protected Resource(string code)
        {
            this.code = code;
            channels = new Bag<IChannel>();
            status = new EnumParameter<ResourceStatus>(nameof(status));
            failure = new Failure();

            Status.ValueChanged += Status_ValueChanged;
        }

        private void Status_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            switch (Status.Value)
            {
                case ResourceStatus.Starting:
                    Logger.Log($"{code} starting", Severity.Info);
                    break;

                case ResourceStatus.Executing:
                    Logger.Log($"{code} executing", Severity.Info);
                    break;

                case ResourceStatus.Stopping:
                    Logger.Log($"{code} stopping", Severity.Info);
                    break;

                case ResourceStatus.Stopped:
                    Logger.Log($"{code} stopped", Severity.Info);
                    break;

                case ResourceStatus.Failure:
                    Logger.Log($"{code} failure: {LastFailure}", Severity.Error);
                    break;
            }
        }

        /// <summary>
        /// The <see cref="Resource"/> status
        /// </summary>
        public EnumParameter<ResourceStatus> Status
        {
            get => status;
            protected set => status = value;
        }

        /// <summary>
        /// The <see cref="Resource"/> last <see cref="IFailure"/>
        /// </summary>
        public IFailure LastFailure => failure;

        /// <summary>
        /// The <see cref="Resource"/> <see cref="System.Type"/>
        /// </summary>
        public Type Type => GetType();

        /// <summary>
        /// The <see cref="Resource"/> value (<see cref="Code"/>) as <see cref="object"/>
        /// </summary>
        public object ValueAsObject
        {
            get => code;
            set
            {
                _ = ValueAsObject;
            }
        }

        /// <summary>
        /// The <see cref="Resource"/> open status
        /// </summary>
        public abstract bool IsOpen { get; }

        /// <summary>
        /// The <see cref="Resource"/> collection of
        /// <see cref="IChannel"/>
        /// </summary>
        public Bag<IChannel> Channels => channels;

        /// <summary>
        /// The <see cref="Resource"/> code
        /// </summary>
        public string Code => code;

        /// <summary>
        /// Start the <see cref="Resource"/>
        /// </summary>
        public abstract Task Start();

        /// <summary>
        /// Stop the <see cref="Resource"/>
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Restart the <see cref="Resource"/>
        /// </summary>
        public abstract Task Restart();

        /// <summary>
        /// Handle the operations to perform in case of an <see cref="Exception"/>
        /// </summary>
        /// <remarks>
        /// This method should be called inside each try-catch block in each concrete classes
        /// that inherits from <see cref="Resource"/>
        /// </remarks>
        /// <param name="ex">The <see cref="Exception"/> occurred</param>
        protected void HandleException(Exception ex)
        {
            failure = new Failure(ex);
            Status.Value = ResourceStatus.Failure;
            Logger.Log(ex);
        }

        /// <summary>
        /// Handle the operations to perform in case of a generic <see cref="Resource"/> failure
        /// </summary>
        /// <remarks>
        /// This method should be called inside each if-else block in each concrete classes
        /// that inherits from <see cref="Resource"/>
        /// </remarks>
        /// <param name="message">The error message</param>
        protected void HandleException(string message)
        {
            failure = new Failure(message);
            Status.Value = ResourceStatus.Failure;
            Logger.Log(message, Severity.Error);
        }
    }
}