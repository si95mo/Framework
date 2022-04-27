using Core.Conditions;
using Core.DataStructures;
using Core.Parameters;
using Hardware;
using System;

namespace Devices
{
    /// <summary>
    /// Describe a generic device.
    /// See also <see cref="IDevice"/>
    /// </summary>
    public abstract class Device : IDevice
    {
        private string code;

        public Bag<IChannel> Channels
        { get; protected set; }

        public Bag<IParameter> Parameters
        { get; protected set; }

        public Bag<ICondition> Conditions
        { get; protected set; }

        /// <summary>
        /// The <see cref="Device"/> code
        /// </summary>
        public string Code => code;

        public Type Type => GetType();

        /// <summary>
        /// The <see cref="Device"/> value as <see cref="object"/>
        /// </summary>
        public object ValueAsObject
        {
            get => code;
            set
            {
                _ = ValueAsObject;
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

            Channels = new Bag<IChannel>();
            Parameters = new Bag<IParameter>();
            Conditions = new Bag<ICondition>();
        }

        /// <summary>
        /// Check if an <see cref="IChannel"/> specified by <paramref name="channelCode"/>
        /// is valid (i.e. found in the <see cref="ServiceBroker"/>)
        /// </summary>
        /// <param name="channelCode">The channel code</param>
        /// <returns><see langword="true"/> if the channel is valid, <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// This method actually evaluate <paramref name="channelCode"/> only if it is not equal to <see langword="null"/>,
        /// otherwise <see langword="false"/> is returned
        /// </remarks>
        protected bool IsAValidChannel(string channelCode)
        {
            bool isValid = false;

            if (channelCode != null)
            {
                IChannel channel = ServiceBroker.Get<IChannel>().Get(channelCode);
                isValid &= channel != null;
            }

            return isValid;
        }

        /// <summary>
        /// Connect the <see cref="Channels"/> to the relative
        /// <see cref="Parameters"/>
        /// </summary>
        protected abstract void ConnectChannelsToParameters();

        /// <summary>
        /// Stop the <see cref="Device"/>.
        /// This method should handle the safety (i.e. should put the <see cref="Device"/>
        /// in a safe condition when something abnormal happens)
        /// </summary>
        public abstract void Stop();

        public override string ToString() => code;
    }
}