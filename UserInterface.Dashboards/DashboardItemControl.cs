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
            layoutPanel.Controls.Add(analogReadControl);

            DigitalReadControl digitalReadControl = new DigitalReadControl();
            digitalReadControl.DoubleClick += DigitalReadControl_DoubleClick;
            digitalReadControl.Draggable(false);
            layoutPanel.AddControl(digitalReadControl);
        }

        private void AnalogReadControl_DoubleClick(object sender, EventArgs e)
        {
            AnalogReadControl control = new AnalogReadControl();
            control.Draggable(true);
            control.Click += (_, __) =>
            {
                configPanel.Controls.Clear();
                configPanel.Controls.Add(new ItemConfigurationControl(control));
            };

            dashboard.Controls.Add(control);
        }

        private void DigitalReadControl_DoubleClick(object sender, EventArgs e)
        {
            DigitalReadControl control = new DigitalReadControl();
            control.Draggable(true);
            control.Click += (_, __) =>
            {
                configPanel.Controls.Clear();
                configPanel.Controls.Add(new ItemConfigurationControl(control));
            };

            dashboard.Controls.Add(control);
        }
    }
}