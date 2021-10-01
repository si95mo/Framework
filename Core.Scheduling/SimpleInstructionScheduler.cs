using Extensions;
using Instructions;
using System;

namespace Core.Scheduling
{
    /// <summary>
    /// Implement a simple <see cref="Instruction"/> scheduler
    /// </summary>
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
        /// stored in the subscriber ones,
        /// and remove it from the <see cref="ActionQueue{T}"/>
        /// </summary>
        /// <returns>The <see cref="Instruction"/> executed</returns>
        public override IInstruction Execute()
        {
            if (lastExecution.Count == 0 && subscribedInstructions.Count != 0)
                foreach (IInstruction i in subscribedInstructions)
                    lastExecution.Enqueue(i.DeepCopy());

            if (subscribedInstructions.Count == 0 && lastExecution.Count != 0)
            {
                foreach (IInstruction i in lastExecution)
                    subscribedInstructions.Enqueue(i.DeepCopy());

                lastExecution.Clear();

                foreach (IInstruction i in subscribedInstructions)
                    lastExecution.Enqueue(i.DeepCopy());
            }

            Instruction instruction = (Instruction)subscribedInstructions.Dequeue();
            instruction.Invoke();

            return instruction;
        }
    }
}