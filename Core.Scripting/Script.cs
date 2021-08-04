using Core.DataStructures;
using System;

namespace Core.Scripting
{
    public abstract class Script : IScript
    {
        protected string code;

        public string Code => code;

        public object ValueAsObject { get => code; set => _ = value; }

        public Type Type => this.GetType();

        /// <summary>
        /// Initialize the new instance with default parameters
        /// </summary>
        /// <param name="code">The code</param>
        protected Script(string code)
        {
            this.code = code;
            ServiceBroker.Add<IScript>(this);
        }

        /// <summary>
        /// Initialize the new instance
        /// </summary>
        protected Script() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// The <see cref="Script"/> to execute
        /// </summary>
        public abstract void Execute();
    }
}