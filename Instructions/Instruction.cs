using Core.DataStructures;
using Core.Parameters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Instructions
{
    /// <summary>
    /// Basic implementation of a generic instruction
    /// </summary>
    [Serializable]
    public abstract class Instruction : IInstruction
    {
        private string code;
        private DateTime startTime, stopTime;
        private Bag<IParameter> inputParameters, outputParameters;

        private int order;

        public string Code => code;

        public Bag<IParameter> InputParameters => inputParameters;

        public Bag<IParameter> OutputParameters => outputParameters;

        public BooleanParameter Succeeded
        {
            get;
            internal set;
        }

        public DateTime StartTime
        {
            get => startTime;
            internal set => startTime = value;
        }

        public DateTime StopTime
        {
            get => stopTime;
            internal set => stopTime = value;
        }

        public BooleanParameter Failed { get; internal set; }

        /// <summary>
        /// The order of the <see cref="Instruction"/> (used in case of parallelism)
        /// </summary>
        public int Order => order;

        public Type Type => GetType();

        /// <summary>
        /// The <see cref="Instruction"/> value as <see cref="object"/>
        /// </summary>
        public object ValueAsObject
        {
            get => code;
            set
            {
                object _ = ValueAsObject;
            }
        }

        /// <summary>
        /// Set the class attributes to default values
        /// </summary>
        /// <param name="code">The code</param>
        protected Instruction(string code)
        {
            this.code = code;
            order = 0;

            inputParameters = new Bag<IParameter>();
            outputParameters = new Bag<IParameter>();

            Succeeded = new BooleanParameter($"{code}.{nameof(Succeeded)}", false);
            Failed = new BooleanParameter($"{code}.{nameof(Failed)}", false);

            outputParameters.Add(Succeeded);
        }

        /// <summary>
        /// Execute the <see cref="Instruction"/>
        /// </summary>
        public abstract Task ExecuteInstruction();

        /// <summary>
        /// Method called on start of the <see cref="Instruction"/>. <br/>
        /// Override to add some logic
        /// </summary>
        /// <remarks>
        /// Note that <see langword="base"/> method should be called
        /// inside the new implementation
        /// </remarks>
        public virtual void OnStart()
            => startTime = DateTime.Now;

        /// <summary>
        /// Method called on stop of the <see cref="Instruction"/>. <br/>
        /// Override to add some logic
        /// </summary>
        /// <remarks>
        /// Note that <see langword="base"/> method should be called
        /// inside the new implementation
        /// </remarks>
        public virtual void OnStop()
        {
            stopTime = DateTime.Now;

            if (!Failed.Value)
                Succeeded.Value = true;
        }

        /// <summary>
        /// Method called on fail of the <see cref="Instruction"/>. <br/>
        /// Override to add some logic
        /// </summary>
        /// <remarks>
        /// Note that <see langword="base"/> method should be called
        /// inside the new implementation
        /// </remarks>
        protected virtual void OnFail()
        {
            stopTime = DateTime.Now;
            Succeeded.Value = false;
        }

        public override string ToString()
        {
            string description = code;

            int index = 0;
            while (index < inputParameters.Count)
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
        /// Let the <see cref="Instruction"/> fail. <br/>
        /// Call in case the <see cref="Instruction"/> logic and/or result are not met
        /// </summary>
        protected void Fail()
        {
            Failed.Value = true;
            OnFail();
        }
    }
}