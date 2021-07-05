using Core.DataStructures;
using Core.Scheduling.Wrapper;
using Instructions;
using System;
using System.Linq;

namespace Core.Scheduling
{
    [Serializable]
    public class SimpleInstructionScheduler : InstructionScheduler
    {
        /// <summary>
        /// Creates a new instance of the <see cref="SimpleMethodScheduler"/>
        /// </summary>
        public SimpleInstructionScheduler() : base()
        { }

        /// <summary>
        /// Executes the <see cref="Action"/> associated with the <see cref="Instruction"/>
        /// stored in the <see cref="Subscribers"/>,
        /// and remove it from the <see cref="ActionQueue{T}"/>
        /// </summary>
        /// <returns>The <see cref="Instruction"/> executed</returns>
        public override IInstruction Execute()
        {
            Instruction instruction = (Instruction)subscribedInstructions.Dequeue();
            instruction.Invoke();

            return instruction;
        }
    }
}