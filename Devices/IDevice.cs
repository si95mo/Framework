using Core;
using Core.DataStructures;

namespace Devices
{
    /// <summary>
    /// Describe a generic device
    /// </summary>
    public interface IDevice : IProperty
    {
        /// <summary>
        /// The <see cref="IDevice"/> collection
        /// of channels. See <see cref="IChannel"/>
        /// and <see cref="Bag{IProperty}"/>
        /// </summary>
        Bag<IChannel> Channels
        { get; }

        /// <summary>
        /// The <see cref="IDevice"/> collection
        /// of channels. See <see cref="IParameter"/>
        /// and <see cref="Bag{T}"/>
        /// </summary>
        Bag<IParameter> Parameters
        { get; }
    }
}