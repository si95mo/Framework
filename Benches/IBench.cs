using Core;
using Core.DataStructures;
using Devices;

namespace Benches
{
    public interface IBench : IProperty
    {
        /// <summary>
        /// The <see cref="IBench"/> collection
        /// of devices. See <see cref="IDevice"/>
        /// and <see cref="Bag{IProperty}"/>
        /// </summary>
        Bag<IDevice> Devices
        { get; }

        /// <summary>
        /// The <see cref="IBench"/> collection
        /// of parameters. See <see cref="IParameter"/>
        /// and <see cref="Bag{T}"/>
        /// </summary>
        Bag<IParameter> Parameters
        { get; }
    }
}