using IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UserInterface.Forms;

namespace UserInterface.Dashboards
{
    public partial class LoadForm : CustomForm
    {
        private Panel dashboard;

        private LoadForm() : base()
        {
            InitializeComponent();
        }

        public LoadForm(Panel dashboard) : this()
        {
            this.dashboard = dashboard;
            txcDashboardName.Focus();
        }

        private async void BtnLoadDashboard_Click(object sender, System.EventArgs e)
        {
            if (txcDashboardName.Text.CompareTo(string.Empty) == 0)
            {
                CustomMessageBox.Show(this, "Warning", "Enter the dashboard name first!");
                txcDashboardName.Focus();
            }
            else
            {
                string json = await FileHandler.ReadAsync($"dashboards//{txcDashboardName.Text}.json");
                List<DashboardControl> items = JsonConvert.DeserializeObject<List<DashboardControl>>(json);

                // TODO: Finish implementation (click event for configuration)
                if (items.Count != 0)
                {
                    dashboard.Controls.Clear();

                    foreach (DashboardControl control in items)
                    {
                        IDashboardControl tmp = (IDashboardControl)Activator.CreateInstance(control.Type);
                        tmp.SetChannel(control.ChannelCode);
                        tmp.Description = control.Description;
                        (tmp as DraggableControl).Size = control.Size;
                        (tmp as DraggableControl).Location = control.Location;

                        dashboard.Controls.Add(tmp as DraggableControl);
                    }
                }
                Close();
                Dispose();
            }
        }
    }
}
