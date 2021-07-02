using Benches.Tasks;
using Core;
using Core.DataStructures;
using Devices;
using System;

namespace Benches
{
    internal class Bench<TDevice, TParameter, TInstruction> : IBench
    {
        private string code;
        private Bag<IDevice> devices;
        private Bag<IParameter> parameters;
        private Configure<TDevice, TParameter, TInstruction> configure;

        /// <summary>
        /// The <see cref="Bench"/> code
        /// </summary>
        public string Code => code;

        /// <summary>
        /// The <see cref="Bench"/> <see cref="Bag{T}"/>
        /// of <see cref="IDevice"/>
        /// </summary>
        public Bag<IDevice> Devices => devices;

        /// <summary>
        /// The <see cref="Bench"/> <see cref="Bag{T}"/>
        /// of <see cref="IParameter"/>
        /// </summary>
        public Bag<IParameter> Parameters => parameters;

        /// <summary>
        /// Initialize the class attributes with
        /// default parameters
        /// </summary>
        protected Bench() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Initialize the class attributes with
        /// default parameters
        /// </summary>
        /// <param name="code">The code</param>
        protected Bench(string code)
        {
            this.code = code;
            devices = new Bag<IDevice>();
            parameters = new Bag<IParameter>();

            configure = new Configure<TDevice, TParameter, TInstruction>(this);
            configure.Execute();
        }

        public override string ToString() => code;
    }
}