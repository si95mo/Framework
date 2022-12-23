using Core.Scripting;
using System;
using System.Windows.Forms;

namespace ScriptTest
{
    public class Test : Script
    {
        public Test() : base(nameof(Test))
        { }

        public override void Run()
        {
            MessageBox.Show($"{DateTime.Now:HH:mm:ss.fff} >> {nameof(Run)} executed");
        }

        public override void Clear()
        {
            MessageBox.Show($"{DateTime.Now:HH:mm:ss.fff} >> {nameof(Clear)} executed");
        }
    }
}