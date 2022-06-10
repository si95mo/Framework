using Core.Scripting;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScriptTest
{
    public class Test : Script
    {
        private readonly string message;

        public Test(string code, string message) : base(code)
        {
            this.message = message;
        }

        public override Task Execute()
        {
            MessageBox.Show($"Script executed!{Environment.NewLine}\t{message}");
            return Task.CompletedTask;
        }
    }
}