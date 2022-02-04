using Core.DataStructures;
using System;

namespace Core.Scripting
{
    /// <summary>
    /// Implement a script class
    /// </summary>
    public abstract class Script : IScript
    {
        /// <summary>
        /// The code
        /// </summary>
        protected string code;

        /// <summary>
        /// The code
        /// </summary>
        public string Code => code;

        /// <summary>
        /// The value as object
        /// </summary>
        public object ValueAsObject { get => code; set => _ = value; }

        /// <summary>
        /// The <see cref="System.Type"/>
        /// </summary>
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