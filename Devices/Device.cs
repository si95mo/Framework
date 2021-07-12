using Core;
using Core.DataStructures;
using Devices.Tasks;
using System;

namespace Devices
{
    /// <summary>
    /// Describe a generic device.
    /// See also <see cref="IDevice"/>
    /// </summary>
    public class Device<TChannel, TParameter> : IDevice
    {
        private string code;
        private Bag<IChannel> channels;
        private Bag<IParameter> parameters;
        private Configure<TChannel, TParameter> configure;

        /// <summary>
        /// The <see cref="Device"/> <see cref="Bag{T}"/> of
        /// <see cref="IChannel"/>
        /// </summary>
        public Bag<IChannel> Channels => channels;

        /// <summary>
        /// The <see cref="Device"/> <see cref="Bag{T}"/> of
        /// <see cref="IParameter"/>
        /// </summary>
        public Bag<IParameter> Parameters => parameters;

        /// <summary>
        /// The <see cref="Device"/> code
        /// </summary>
        public string Code => code;

        public Type Type => this.GetType();

        /// <summary>
        /// The <see cref="Device{TChannel, TParameter}"/> value as <see cref="object"/>
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
        protected Device() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Initialize the class attributes with
        /// default parameters
        /// </summary>
        /// <param name="code">The code</param>
        protected Device(string code)
        {
            this.code = code;
            channels = new Bag<IChannel>();
            parameters = new Bag<IParameter>();

            configure = new Configure<TChannel, TParameter>(this);
            configure.Execute();
        }

        public override string ToString() => code;
    }
}