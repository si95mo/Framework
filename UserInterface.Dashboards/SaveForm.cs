using IO;
using Newtonsoft.Json;
using System;
using System.Windows.Forms;
using UserInterface.Forms;

namespace UserInterface.Dashboards
{
    /// <summary>
    /// Handle the dashboard save process
    /// </summary>
    public partial class SaveForm : CustomForm
    {
        private Panel dashboard;

        private SaveForm() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Create a new instance of <see cref="SaveForm"/>
        /// </summary>
        /// <param name="dashboard">The dashboard to save</param>
        public  SaveForm(Panel dashboard) : this()
        {
            this.dashboard = dashboard;
        }

        private async void BtnSaveDashboard_Click(object sender, EventArgs e)
        {
            if (txcDashboardName.Text.CompareTo(string.Empty) == 0)
            {
                CustomMessageBox.Show(this, "Warning", "Enter the dashboard name first!");
                txcDashboardName.Focus();
            }
            else
            {
                IOUtility.CreateDirectoryIfNotExists("dashboards");

                string serializedDashboard = "";
                foreach (Control control in dashboard.Controls)
                {
                    if (control is IDashboardControl)
                    {
                        serializedDashboard += JsonConvert.SerializeObject(
                            control as IDashboardControl,
                            
                            Formatting.Indented,
                            new JsonSerializerSettings()
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            }
                        );
                        serializedDashboard += Environment.NewLine;
                    }
                }

                await FileHandler.SaveAsync(serializedDashboard, $"dashboards//{txcDashboardName.Text}");

                Close();
                Dispose();
            }
        }
    }
}
