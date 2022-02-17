using Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Instructions.Common
{
    /// <summary>
    /// Implement an <see cref="Instruction"/> that is a collection of <see cref="Sequence"/>
    /// that has to be executed in parallel
    /// </summary>
    public class Parallel : Sequence
    {
        private List<IInstruction> instructions;

        public Parallel(string code, int order) : base(code, order)
        {
            instructions = new List<IInstruction>();
            Order = order;
        }

        /// <summary>
        /// Add an <see cref="IInstruction"/> to the <see cref="Parallel"/> sub-instructions
        /// </summary>
        /// <param name="instruction">The instruction to add</param>
        public new void Add(IInstruction instruction)
        {
            instruction.Order = Order;
            instructions.Add(instruction);
        }

        public override async Task ExecuteInstruction()
        {
            instructions.ForEach(x => (x as Instruction).OnStart());
            await instructions.ParallelForeachAsync(async (x) => await x.ExecuteInstruction());
            instructions.ForEach(x => (x as Instruction).OnStop());

            bool flag = false;
            for (int i = 0; i < instructions.Count && !flag; i++)
            {
                if (instructions[i].Failed.Value)
                {
                    flag = true;
                    Fail();
                }
                else
                    Succeeded.Value = true;
            }
        }
    }
}