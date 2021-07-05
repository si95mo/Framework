using Instructions;
using System;

namespace Benches.Template.Actions
{
    public class Instructions
    {
        private static DummyInstruction dummy;

        // Fore each channel, there must be a public property with the getter
        public static DummyInstruction Dummy
        {
            get => dummy;
            set => dummy = value;
        }
    }
}