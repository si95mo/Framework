using Core.DataStructures;
using Hardware;
using System;

namespace Devices
{
    /// <summary>
    /// Describe a generic device.
    /// See also <see cref="IDevice"/>
    /// </summary>
    class Device : IDevice
    {
        private string code;
        private Bag<IChannel> channels;

        /// <summary>
        /// The <see cref="Device"/> <see cref="Bag{IProperty}"/> of 
        /// <see cref="IChannel"/>
        /// </summary>
        public Bag<IChannel> Channels => channels;

        /// <summary>
        /// The <see cref="Device"/> code
        /// </summary>
        public string Code => code;

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
        }
    }
}
