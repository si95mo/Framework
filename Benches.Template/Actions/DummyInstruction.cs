using Benches;
using Core.DataStructures;
using Core.Parameters;
using Devices;
using Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benches.Template.Actions
{
    [Serializable]
    public class DummyInstruction : Instruction
    {
        private StringParameter Message = new StringParameter(nameof(Message));

        public DummyInstruction() : base(Guid.NewGuid().ToString(), new Bag<IDevice>())
        { }

        public DummyInstruction(string code, IBench bench) : base(code, bench.Devices)
        {
            InputParameters.Add(Message);
        }

        public override void Invoke()
        {
            Console.WriteLine(Message.Value);
        }
    }
}
