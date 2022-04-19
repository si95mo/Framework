using Extensions;
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
        private Panel configPanel;

        private LoadForm() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Create a new instance of <see cref="LoadForm"/>
        /// </summary>
        /// <param name="dashboard">The dashboard <see cref="Panel"/></param>
        /// <param name="configPanel">The configuration <see cref="Panel"/></param>
        public LoadForm(Panel dashboard, Panel configPanel) : this()
        {
            this.dashboard = dashboard;
            this.configPanel = configPanel;

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

                        HandleNewControl(tmp);
                    }
                }

                Close();
                Dispose();
            }
        }

        /// <summary>
        /// Handle the new created <see cref="IDashboardControl"/>
        /// </summary>
        /// <param name="control">The <see cref="IDashboardControl"/> to handle</param>
        private void HandleNewControl(IDashboardControl control)
        {
            (control as DraggableControl).SetDraggable(true);
            (control as DraggableControl).Click += (_, __) =>
            {
                configPanel.Controls.Clear();
                configPanel.Controls.Add(new ItemConfigurationControl(control));
            };

            dashboard.Controls.Add(control as DraggableControl);
        }
    }
}