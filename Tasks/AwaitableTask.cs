using Core;
using Core.DataStructures;
using Core.Parameters;
using System;
using System.Threading.Tasks;

namespace Tasks
{
    /// <summary>
    /// Implement generic aspects of the <see cref="IAwaitable"/> interface
    /// </summary>
    public abstract class AwaitableTask : IAwaitable, IProperty
    {
        private string code;

        public abstract EnumParameter<TaskStatus> Status { get; protected set; }
        public abstract StringParameter Message { get; protected set; }

        public string Code => code;
        public object ValueAsObject { get => Status.Value; set => _ = value; }

        public Type Type => GetType();

        public Bag<IParameter> InputParameters { get; protected set; }

        public Bag<IParameter> OutputParameters { get; protected set; }

        /// <summary>
        /// Create a new instance of <see cref="AwaitableTask"/>
        /// </summary>
        /// <param name="code">The code</param>
        protected AwaitableTask(string code)
        {
            this.code = code;

            Status = new EnumParameter<TaskStatus>($"{Code}.{nameof(Status)}", TaskStatus.Created);
            Message = new StringParameter($"{Code}.{nameof(Message)}", "");

            InputParameters = new Bag<IParameter>();
            OutputParameters = new Bag<IParameter>();
        }

        public abstract Task Start();
    }
}