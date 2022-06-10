using Extensions;
using Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Scheduling
{
    /// <summary>
    /// Implement an <see cref="Instruction"/> scheduler
    /// </summary>
    [Serializable]
    public class InstructionScheduler : IScheduler<IInstruction>
    {
        private SortedDictionary<int, Queue<IInstruction>> instructions;
        private bool stop;

        /// <summary>
        /// The subscribed <see cref="Instruction"/>
        /// </summary>
        public ActionQueue<IInstruction> Instructions
        {
            get
            {
                ActionQueue<IInstruction> instructionQueue = new ActionQueue<IInstruction>();

                foreach (Queue<IInstruction> instruction in instructions.Values)
                    instruction.ToList().ForEach(x => instructionQueue.Enqueue(x));

                return instructionQueue;
            }
        }

        /// <summary>
        /// Create a new instance of <see cref="InstructionScheduler"/>
        /// </summary>
        public InstructionScheduler()
        {
            instructions = new SortedDictionary<int, Queue<IInstruction>>();
            stop = false;
        }

        /// <summary>
        /// Add an <see cref="Instruction"/> to the
        /// <see cref="Instructions"/>
        /// </summary>
        /// <param name="instruction">The <see cref="Instruction"/> to add</param>
        public void Add(IInstruction instruction)
        {
            if (instructions.ContainsKey(instruction.Order))
                instructions[instruction.Order].Enqueue(instruction);
            else
            {
                instructions.Add(instruction.Order, new Queue<IInstruction>());
                instructions[instruction.Order].Enqueue(instruction);
            }
        }

        /// <summary>
        /// Execute all the subscribed <see cref="Instruction"/>
        /// and remove them from <see cref="Instructions"/>
        /// </summary>
        /// <returns>The <see cref="List{T}"/> with the executed <see cref="Instruction"/></returns>
        public async Task<List<IInstruction>> Execute()
        {
            stop = false;

            // First instruction order to execute
            int order = instructions.Keys.Min();
            List<IInstruction> parallelInstructions = new List<IInstruction>();
            List<IInstruction> executedInstructions = new List<IInstruction>();

            while (instructions.Count > 0 && !stop)
            {
                // Instruction order handling
                while (instructions[order].Count > 0)
                    parallelInstructions.Add(instructions[order].Dequeue());

                // Parallel execution of the instruction with the same order
                parallelInstructions.ForEach(x => (x as Instruction).OnStart()); // On start logic
                await parallelInstructions.ParallelForeachAsync(async (x) => await x.ExecuteInstruction()); // Execution
                parallelInstructions.ForEach(x => (x as Instruction).OnStop()); // On stop logic

                // Remove the order of executed instruction and update it with the new one
                instructions.Remove(order);
                order = instructions.Count > 0 ? instructions.Keys.Min() : 0;

                executedInstructions.AddRange(parallelInstructions);
                parallelInstructions.Clear();
            }

            return executedInstructions;
        }

        /// <summary>
        /// Stop (pause) the current execution
        /// </summary>
        public void StopAll() => stop = true;

        public void RemoveAll()
        {
            instructions.Clear();
        }

        // TODO: implement the method (save execution list for InstructionScheduler)
        public void SaveExecutionList(string fileName)
        {
            throw new NotImplementedException();
        }

        // TODO: implement the method (load execution list for InstructionScheduler)
        public void LoadExecutionList(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}