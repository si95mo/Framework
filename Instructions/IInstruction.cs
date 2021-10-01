using Core;
using Core.DataStructures;
using Core.Parameters;
using Devices;

namespace Instructions
{
    /// <summary>
    /// Describe a generic instruction
    /// </summary>
    public interface IInstruction : IProperty
    {
        /// <summary>
        /// The <see cref="Bag{T}"/>
        /// containing all the available <see cref="Method"/>
        /// </summary>
        Bag<Method> Methods
        { get; }

        /// <summary>
        /// The <see cref="Bag{T}"/> of all the <see cref="IDevice"/>
        /// with <see cref="IInstruction"/> related methods
        /// </summary>
        Bag<IDevice> Devices
        { get; set; }

        /// <summary>
        /// The <see cref="IInstruction"/> <see cref="Bag{T}"/>
        /// of input <see cref="IParameter"/>
        /// </summary>
        Bag<IParameter> InputParameters
        { get; }

        /// <summary>
        /// The <see cref="IInstruction"/> <see cref="Bag{T}"/>
        /// of output <see cref="IParameter"/>
        /// </summary>
        Bag<IParameter> OutputParameters
        { get; }

        /// <summary>
        /// Execute the <see cref="IInstruction"/>
        /// </summary>
        void Invoke();
    }
}