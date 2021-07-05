using Core.DataStructures;
using Core.Parameters;
using Devices;
using Instructions;
using System;

namespace Benches.Template.Actions
{
    [Serializable]
    public class DummyInstruction : Instruction
    {
        private StringParameter Message = new StringParameter(nameof(Message));
        private BooleanParameter Switch = new BooleanParameter(nameof(Switch));

        private NumericParameter Result = new NumericParameter(nameof(Result));
        private BooleanParameter NotSwitch = new BooleanParameter(nameof(NotSwitch));

        public DummyInstruction() : base(Guid.NewGuid().ToString(), new Bag<IDevice>())
        { }

        public DummyInstruction(string code, IBench bench) : base(code, bench.Devices)
        {
            InputParameters.Add(Message);
            InputParameters.Add(Switch);

            OutputParameters.Add(Result);
            OutputParameters.Add(NotSwitch);
        }

        public override void Invoke()
        {
            if (Switch.Value)
                Console.WriteLine($"{Switch} :: {Message.Value}");
            else
                Console.WriteLine($"{Switch} :: {Message.Value}");

            NotSwitch.Value = !Switch.Value;
            Result.Value = new Random(Guid.NewGuid().GetHashCode()).Next();
        }
    }
}