using System;
using System.Windows.Forms;

namespace Core.Scheduling.Tests
{
    internal class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new TestForm());
        }
    }
}