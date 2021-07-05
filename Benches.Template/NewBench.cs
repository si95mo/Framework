using Benches.Template.Actions;
using Core;
using Core.DataStructures;
using Devices;
using Devices.Template;
using System;

namespace Benches.Template
{
    public class NewBench : Bench<Devices.Devices, Parameters.Parameters, Actions.Instructions>
    {
        /// <summary>
        /// Create a new instance of <see cref="NewBench"/>
        /// </summary>
        /// <param name="code">The code</param>
        public NewBench(string code) : base(code)
        {
            Actions.Instructions.Dummy = new DummyInstruction("DummyInstruction", this);
            Instructions.Add(Actions.Instructions.Dummy);
        }
    }
}