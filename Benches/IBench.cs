using Core;
using Core.DataStructures;
using Core.Parameters;
using Devices;
using Instructions;

namespace Benches
{
    /// <summary>
    /// Define a bench general prototype
    /// </summary>
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

        /// <summary>
        /// The <see cref="IBench"/> collection
        /// of instructions. See <see cref="IInstruction"/>
        /// and <see cref="Bag{T}"/>
        /// </summary>
        Bag<IInstruction> Instructions
        { get; }
    }
}