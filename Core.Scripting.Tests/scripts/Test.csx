using Core.Scripting;
using System;
using System.Windows.Forms;

namespace ScriptTest
{
    public class Test : Script
    {
        private string message;

        public Test(string code, string message) : base(code)
        {
            this.message = message;
        }

        public override void Execute()
        {
            MessageBox.Show($"Script executed!{Environment.NewLine}\t{message}");
        }
    }
}