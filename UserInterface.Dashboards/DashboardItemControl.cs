using System;
using System.Windows.Forms;

namespace UserInterface.Dashboards
{
    /// <summary>
    /// Implement an <see cref="IDashboardControl"/> item creation <see cref="UserControl"/>
    /// </summary>
    public partial class DashboardItemControl : UserControl
    {
        private Panel dashboard, configPanel;

        /// <summary>
        /// Create a new instance of <see cref="DashboardItemControl"/>
        /// </summary>
        /// <param name="dashboard"></param>
        /// <param name="configPanel"></param>
        public DashboardItemControl(Panel dashboard, Panel configPanel)
        {
            InitializeComponent();

            this.dashboard = dashboard;
            this.configPanel = configPanel;

            AnalogReadControl analogReadControl = new AnalogReadControl();
            analogReadControl.DoubleClick += AnalogReadControl_DoubleClick;
            analogReadControl.Draggable(false);

            layoutPanel.AddControl(analogReadControl);
        }

        private void AnalogReadControl_DoubleClick(object sender, EventArgs e)
        {
            AnalogReadControl analogControl = new AnalogReadControl();
            analogControl.Draggable(true);
            analogControl.Click += (_, __) =>
            {
                configPanel.Controls.Clear();
                configPanel.Controls.Add(new ItemConfigurationControl(analogControl));
            };

            dashboard.Controls.Add(analogControl);
        }
    }
}