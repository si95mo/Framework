using Core;
using Core.DataStructures;
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
        protected string code;
        protected ResourceStatus status;
        protected IFailure failure;
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
        }

        /// <summary>
        /// The <see cref="Resource"/> status
        /// </summary>
        public ResourceStatus Status
        {
            get => status;
            protected set
            {
                // Eventually trigger the value changed event
                if (value != status)
                {
                    ResourceStatus oldStatus = status;
                    status = value;
                    OnStatusChanged(new StatusChangedEventArgs(oldStatus, status));
                }
            }
        }

        /// <summary>
        /// The <see cref="Resource"/> <see cref="Status"/> value
        /// changed event handler
        /// </summary>
        public EventHandler<StatusChangedEventArgs> StatusChangedHandler;

        /// <summary>
        /// The <see cref="StatusChangedHandler"/> event handler
        /// for the <see cref="Status"/> property
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> StatusChanged
        {
            add
            {
                lock (objectLock)
                    StatusChangedHandler += value;
            }

            remove
            {
                lock (objectLock)
                    StatusChangedHandler -= value;
            }
        }

        /// <summary>
        /// On status changed event
        /// </summary>
        /// <param name="e">The <see cref="StatusChangedEventArgs"/></param>
        protected virtual void OnStatusChanged(StatusChangedEventArgs e)
            => StatusChangedHandler?.Invoke(this, e);

        /// <summary>
        /// The <see cref="Resource"/> last <see cref="IFailure"/>
        /// </summary>
        public IFailure LastFailure => failure;

        /// <summary>
        /// The <see cref="Resource"/> <see cref="System.Type"/>
        /// </summary>
        public Type Type => GetType();

        /// <summary>
        /// The <see cref="Resource"/> value as <see cref="object"/>
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
        public abstract void Restart();
    }
}