using Extensions;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace UserInterface.Dashboards
{
    [ComVisible(true)]
    [Guid("CD46781D-B691-4287-B802-C9E2540AF08A")]
    [ProgId("WpfComHostDemo.WinformHostUserControl")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    public partial class DashboardPanel : UserControl
    {
        /// <summary>
        /// The actual <see cref="DashboardPanel"/> dashboard
        /// </summary>
        public Panel Dashboard => dashboard;

        private readonly Size dashboardSize;
        private readonly Point dashboardLocation;

        public DashboardPanel()
        {
            InitializeComponent();

            dashboardSize = dashboard.Size;
            dashboardLocation = dashboard.Location;

            dashboard.BringToFront();

            DashboardItemControl itemControl = new DashboardItemControl(dashboard, configPanel);
            itemControl.Size = itemPanel.Size;
            itemPanel.Controls.Add(itemControl);
        }

        private void BtnStart_Click(object sender, System.EventArgs e)
        {
            dashboard.Location = new Point(0, dashboardLocation.Y);
            dashboard.Size = new Size(Width, dashboard.Height);

            foreach (Control control in dashboard.Controls)
            {
                if (control is DraggableControl)
                    (control as DraggableControl).SetDraggable(false);
            }
        }

        private void BtnStop_Click(object sender, System.EventArgs e)
        {
            dashboard.Location = dashboardLocation;
            dashboard.Size = dashboardSize;

            foreach (Control control in dashboard.Controls)
            {
                if (control is DraggableControl)
                    (control as DraggableControl).SetDraggable(true);
            }
        }

        private void BtnSave_Click(object sender, System.EventArgs e)
        {
            SaveForm form = new SaveForm(dashboard);

            form.ShowInTaskbar = false;
            form.ShowIcon = false;
            form.ShowDialog();
        }

        private void BtnLoad_Click(object sender, System.EventArgs e)
        {
            LoadForm form = new LoadForm(dashboard, configPanel);

            form.ShowInTaskbar = false;
            form.ShowIcon = false;
            form.ShowDialog();
        }
    }
}