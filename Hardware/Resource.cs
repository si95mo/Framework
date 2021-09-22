using Core;
using Core.DataStructures;
using Core.Parameters;
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
        protected EnumParameter<ResourceStatus> status;
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
            status = new EnumParameter<ResourceStatus>(nameof(status));
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