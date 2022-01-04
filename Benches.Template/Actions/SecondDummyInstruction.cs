using Core.Parameters;
using Instructions;
using System;
using System.Threading.Tasks;

namespace Benches.Template.Actions
{
    [Serializable]
    public enum InstructionEnum
    {
        None = 0,
        First = 1,
        Second = 2,
        Third = 3
    }

    [Serializable]
    public class SecondDummyInstruction : Instruction
    {
        private EnumParameter<InstructionEnum> Switch = new EnumParameter<InstructionEnum>(nameof(Switch));

        public SecondDummyInstruction() : base(Guid.NewGuid().ToString())
        { }

        public SecondDummyInstruction(string code, IBench bench) : base(code)
        {
            InputParameters.Add(Switch);
        }

        public override async Task ExecuteInstruction()
        {
            switch (Switch.Value)
            {
                case InstructionEnum.None:
                    Console.WriteLine($"{Switch} :: {Switch.Value} (N)");
                    break;

                case InstructionEnum.First:
                    Console.WriteLine($"{Switch} :: {Switch.Value} (F)");
                    break;

                case InstructionEnum.Second:
                    Console.WriteLine($"{Switch} :: {Switch.Value} (S)");
                    break;

                case InstructionEnum.Third:
                    Console.WriteLine($"{Switch} :: {Switch.Value} (T)");
                    break;
            }

            await Task.Delay(10);
        }
    }
}