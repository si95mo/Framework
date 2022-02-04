using System.Collections.Generic;
using System.Threading.Tasks;

namespace Instructions.Common
{
    /// <summary>
    /// Implement an <see cref="Instruction"/> that is a sequence of sub-instructions
    /// </summary>
    public class Sequence : Instruction
    {
        private List<IInstruction> instructions;

        public Sequence(string code, int order) : base(code)
        {
            instructions = new List<IInstruction>();
            Order = order;
        }

        /// <summary>
        /// Add an <see cref="IInstruction"/> to the <see cref="Sequence"/> sub-instructions
        /// </summary>
        /// <param name="instruction">The instruction to add</param>
        public void Add(IInstruction instruction)
        {
            instruction.Order = Order;
            instructions.Add(instruction);
        }

        public override async Task ExecuteInstruction()
        {
            foreach (IInstruction instruction in instructions)
            {
                (instruction as Instruction).OnStart();
                await instruction.ExecuteInstruction();
                (instruction as Instruction).OnStop();

                if (instruction.Failed.Value)
                    Fail();
                else
                    Succeeded.Value = true;
            }
        }
    }
}