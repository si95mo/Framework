using Diagnostic;
using System;
using System.Windows.Forms;

namespace UserInterface.Dashboards.Tests
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Logger.Initialize();

            Logger.Info("Application starting...");
            Application.ApplicationExit += (object sender, EventArgs e) => Logger.Info("Application stopped");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TestForm());
        }
    }
}