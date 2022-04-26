using Core;
using Core.Parameters;
using System;
using System.Threading.Tasks;

namespace Tasks
{
    /// <summary>
    /// Implement generic aspect fo the <see cref="IAwaitable"/> interface
    /// </summary>
    public abstract class Awaitable : IAwaitable, IProperty
    {
        private string code;

        public abstract EnumParameter<TaskStatus> Status { get; protected set; }
        public abstract StringParameter Message { get; protected set; }

        public string Code => code;
        public object ValueAsObject { get => Status.Value; set => _; }

        public Type Type => GetType();

        protected Awaitable (string code)
        {
            this.code = code;
        }

        public abstract Task Start();
    }
}
