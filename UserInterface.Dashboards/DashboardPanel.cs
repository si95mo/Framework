using System.Windows.Forms;

namespace UserInterface.Dashboards
{
    public partial class DashboardPanel : UserControl
    {
        /// <summary>
        /// The actual <see cref="DashboardPanel"/> dashboard
        /// </summary>
        public Panel Dashboard => dashboard;

        public DashboardPanel()
        {
            InitializeComponent();

            DashboardItemControl itemControl = new DashboardItemControl(dashboard);
            itemPanel.Controls.Add(itemControl);
        }
    }
}