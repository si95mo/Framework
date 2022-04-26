using Core;
using Core.Parameters;
using System;
using System.Threading.Tasks;

namespace Tasks
{
    /// <summary>
    /// Implement generic aspects of the <see cref="IAwaitable"/> interface
    /// </summary>
    public abstract class Awaitable : IAwaitable, IProperty
    {
        private string code;

        public abstract EnumParameter<TaskStatus> Status { get; protected set; }
        public abstract StringParameter Message { get; protected set; }

        public string Code => code;
        public object ValueAsObject { get => Status.Value; set => _; }

        public Type Type => GetType();

        /// <summary>
        /// Create a new instace of <see cref="Awaitable"/>
        /// </summary>
        /// <param name="code">The code</param>
        protected Awaitable (string code)
        {
            this.code = code;

            Status = new EnumParameter<TaskStatus>($"{Code}.{nameof(Status)}", TaskStatus.Created);
            Message = new StringParameter($"{Code}.{nameof(Message)}", "");
        }

        public abstract Task Start();
    }
}
