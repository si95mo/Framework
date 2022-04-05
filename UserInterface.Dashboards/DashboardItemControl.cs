using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.Dashboards
{
    public partial class DashboardItemControl : UserControl
    {
        private Panel dashboard;

        public DashboardItemControl(Panel dashboard)
        {
            InitializeComponent();

            this.dashboard = dashboard;

            AnalogReadControl analogReadControl = new AnalogReadControl();
            analogReadControl.DoubleClick += AnalogReadControl_DoubleClick;
            analogReadControl.Draggable(false);

            layoutPanel.AddControl(analogReadControl);
        }

        private void AnalogReadControl_DoubleClick(object sender, EventArgs e)
        {
            AnalogReadControl analogControl = new AnalogReadControl();
            analogControl.Draggable(true);

            dashboard.Controls.Add(analogControl);
        }
    }
}
