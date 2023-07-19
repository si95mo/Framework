using System;
using TwinCAT.Ads.TypeSystem;

namespace Hardware.Twincat
{
    /// <summary>
    /// Describe a generic Twincat channel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class TwincatChannel<T> : Channel<T>, ITwincatChannel
    {
        private string variableName;

        protected object LockObject = new object();

        protected Symbol Symbol;
        protected IResource Resource;
        protected Type ManagedType;

        public string VariableName { get => variableName; protected set => variableName = value; }

        /// <summary>
        /// Create a new instance of <see cref="TwincatChannel{T}"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="variableName">The variable name in Twincat</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        /// <param name="channelType">The <see cref="ChannelType"/></param>
        protected TwincatChannel(string code, string variableName, IResource resource, string measureUnit, string format, ChannelType channelType)
            : base(code, measureUnit, format)
        {
            this.variableName = variableName;
            Resource = resource;
            Resource.Channels.Add(this);
            ChannelType = channelType;
        }

        public abstract void Attach();
    }
}