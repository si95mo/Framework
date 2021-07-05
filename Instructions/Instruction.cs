using Core;
using Core.DataStructures;
using Core.Scheduling.Wrapper;
using Devices;
using System;
using System.Linq;

namespace Instructions
{
    /// <summary>
    /// Basic implementation of a generic instruction
    /// </summary>
    [Serializable]
    public abstract class Instruction : IInstruction
    {
        protected string code;
        protected Bag<Method> methods;
        protected Bag<IDevice> devices;
        private Bag<IParameter> inputParameters, outputParameters;

        /// <summary>
        /// The <see cref="Instruction"/> code
        /// </summary>
        public string Code => code;

        /// <summary>
        /// The <see cref="Instruction"/> <see cref="Bag{T}"/>
        /// containing all the available <see cref="Method"/>
        /// </summary>
        public virtual Bag<Method> Methods => methods;

        /// <summary>
        /// The <see cref="Instruction"/> <see cref="Bag{T}"/>
        /// of input <see cref="IParameter"/>
        /// </summary>
        public virtual Bag<IParameter> InputParameters => inputParameters;

        /// <summary>
        /// The <see cref="Instruction"/> <see cref="Bag{T}"/>
        /// of output <see cref="IParameter"/>
        /// </summary>
        public virtual Bag<IParameter> OutputParameters => outputParameters;

        /// <summary>
        /// The <see cref="Bag{T}"/> of all the <see cref="IDevice"/>
        /// with <see cref="Instruction"/> related methods
        /// </summary>
        public virtual Bag<IDevice> Devices
        {
            get => devices;
            set => devices = value;
        }

        /// <summary>
        /// Set the class attributes to default values
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="devices">The <see cref="Bag{T}"/> of devices</param>
        protected Instruction(string code, Bag<IDevice> devices)
        {
            this.code = code;
            this.devices = devices;

            inputParameters = new Bag<IParameter>();
            outputParameters = new Bag<IParameter>();

            foreach (IDevice device in devices.Values)
                foreach (Method m in MethodWrapper.Wrap(device))
                    methods.Add(m);
        }

        public override string ToString()
        {
            string description = code;

            int index = 0;
            while(index < inputParameters.Count)
            {
                description += Environment.NewLine;
                description += "\tIn: " + inputParameters.ToList().ElementAt(index++).ToString();
            }

            index = 0;
            while (index < outputParameters.Count)
            {
                description += Environment.NewLine;
                description += "\tOut: " + outputParameters.ToList().ElementAt(index++).ToString();
            }

            return description;
        }
        /// <summary>
        /// Execute the <see cref="Instruction"/>
        /// </summary>
        public abstract void Invoke();
    }
}