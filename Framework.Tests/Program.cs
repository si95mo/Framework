using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Framework.Tests
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static async Task Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm form = new MainForm();
            await Manager.InitializeAsync(form: form);

            Application.Run(form);
        }
    }
}
