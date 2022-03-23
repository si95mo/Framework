using Core;
using Core.DataStructures;
using Core.Parameters;
using System;
using System.Threading.Tasks;

namespace Instructions
{
    /// <summary>
    /// Describe a generic instruction
    /// </summary>
    public interface IInstruction : IProperty
    {
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
        /// The start time of the <see cref="IInstruction"/>
        /// </summary>
        DateTime StartTime
        { get; }

        /// <summary>
        /// The stop time of the <see cref="IInstruction"/>
        /// </summary>
        DateTime StopTime
        { get; }

        /// <summary>
        /// The <see cref="IInstruction"/> succeeded property
        /// </summary>
        BoolParameter Succeeded
        { get; }

        /// <summary>
        /// The <see cref="IInstruction"/> failed property
        /// </summary>
        BoolParameter Failed
        { get; }

        /// <summary>
        /// The <see cref="IInstruction"/> order (for parallelism)
        /// </summary>
        int Order
        { get; set; }

        /// <summary>
        /// Execute the <see cref="IInstruction"/>
        /// </summary>
        Task ExecuteInstruction();
    }
}