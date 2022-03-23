using Core.Parameters;
using Instructions;
using System;
using System.Threading.Tasks;

namespace Benches.Template.Actions
{
    [Serializable]
    public class DummyInstruction : Instruction
    {
        private StringParameter Message = new StringParameter(nameof(Message));
        private BoolParameter Switch = new BoolParameter(nameof(Switch));

        private NumericParameter Result = new NumericParameter(nameof(Result));
        private BoolParameter NotSwitch = new BoolParameter(nameof(NotSwitch));

        public DummyInstruction() : base(Guid.NewGuid().ToString())
        { }

        public DummyInstruction(string code, IBench bench) : base(code)
        {
            InputParameters.Add(Message);
            InputParameters.Add(Switch);

            OutputParameters.Add(Result);
            OutputParameters.Add(NotSwitch);
        }

        public override async Task ExecuteInstruction()
        {
            if (Switch.Value)
                Console.WriteLine($"{Switch} :: {Message.Value}");
            else
                Console.WriteLine($"{Switch} :: {Message.Value}");

            NotSwitch.Value = !Switch.Value;
            Result.Value = new Random(Guid.NewGuid().GetHashCode()).Next();

            await Task.Delay(10);
        }
    }
}