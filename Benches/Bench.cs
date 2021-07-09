using Benches.Tasks;
using Core;
using Core.DataStructures;
using Devices;
using Instructions;
using System;

namespace Benches
{
    public abstract class Bench<TDevice, TParameter, TInstruction> : IBench
    {
        protected string code;
        protected Bag<IDevice> devices;
        protected Bag<IParameter> parameters;
        protected Bag<IInstruction> instructions;
        protected Configure<TDevice, TParameter, TInstruction> configure;

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
        /// The <see cref="Bench"/> <see cref="Bag{T}"/>
        /// of <see cref="IInstruction"/>
        /// </summary>
        public Bag<IInstruction> Instructions => instructions;

        public Type Type => this.GetType();

        /// <summary>
        /// The <see cref="Bench{TDevice, TParameter, TInstruction}"/>
        /// value as <see cref="object"/>
        /// </summary>
        public object ValueAsObject
        {
            get => code;
            set
            {
                object v = ValueAsObject;
            }
        }

        /// <summary>
        /// Initialize the class attributes with
        /// default parameters
        /// </summary>
        protected Bench() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Initialize the class attributes
        /// </summary>
        /// <param name="code">The code</param>
        protected Bench(string code)
        {
            this.code = code;
            devices = new Bag<IDevice>();
            parameters = new Bag<IParameter>();
            instructions = new Bag<IInstruction>();

            configure = new Configure<TDevice, TParameter, TInstruction>(this);
            configure.Execute();
        }

        public override string ToString() => code;
    }
}