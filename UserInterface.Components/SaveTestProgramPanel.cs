using Core.Scheduling;
using Diagnostic;

//using IO.File;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    /// <summary>
    /// Implement a save test program panel
    /// </summary>
    public partial class SaveTestProgramPanel : UserControl
    {
        private const string filePath = @"test\";

        //private JObject jsonToSave;
        private InstructionScheduler scheduler;

        /// <summary>
        /// Create a new instance of <see cref="SaveTestProgramPanel"/>
        /// </summary>
        /// <param name="jsonToSave">The <see cref="JObject"/> to save</param>
        /// <param name="scheduler">The <see cref="InstructionScheduler"/></param>
        /// <param name="parent">The <see cref="BaseControl"/> panel</param>
        public SaveTestProgramPanel(JObject jsonToSave, InstructionScheduler scheduler, BaseControl parent)
        {
            InitializeComponent();

            Location = new Point(
                (Screen.PrimaryScreen.WorkingArea.Width - Width) / 2,
                (Screen.PrimaryScreen.WorkingArea.Height - Height) / 2
            );

            //this.jsonToSave = jsonToSave;
            this.scheduler = scheduler;

            parent.Parent.Controls.Add(this);
            BringToFront();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void BtnSaveClick(object sender, EventArgs e)
        {
            if (txbTestProgramName.Text.ToString().CompareTo("") == 0)
            {
                btnSave.FlatAppearance.BorderColor = Colors.Red;
                txbTestProgramName.Focus();
            }
            else
            {
                btnSave.FlatAppearance.BorderColor = Colors.Black;

                string fileName = $"{filePath}{txbTestProgramName.Text}";

                try
                {
                    //JSON.SaveJSON(jsonToSave, fileName + ".json");
                    scheduler.SaveExecutionList(fileName + ".bin");

                    Logger.Log($"Test program with name \"{fileName}\" save correctly", Severity.Info);
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }

                Dispose();
            }
        }
    }
}