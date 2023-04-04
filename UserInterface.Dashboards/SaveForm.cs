using IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public SaveForm(Panel dashboard) : this()
        {
            this.dashboard = dashboard;
            txcDashboardName.Focus();
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
                IoUtility.CreateDirectoryIfNotExists("dashboards");

                string serializedDashboard = "";
                List<DashboardControl> items = new List<DashboardControl>();
                foreach (Control control in dashboard.Controls)
                {
                    if (control is IDashboardControl)
                    {
                        IDashboardControl dashboardControl = control as IDashboardControl;
                        DraggableControl draggableControl = control as DraggableControl;
                        items.Add(
                            new DashboardControl(
                                dashboardControl.ChannelCode,
                                dashboardControl.Description,
                                dashboardControl.GetType(),
                                draggableControl.Size,
                                draggableControl.Location
                            )
                        );
                    }
                }

                serializedDashboard = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(items, Formatting.Indented));
                await FileHandler.SaveAsync(serializedDashboard, $"dashboards//{txcDashboardName.Text}.json");

                Close();
                Dispose();
            }
        }
    }
}