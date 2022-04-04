using System;
using System.Windows.Forms;

namespace UserInterface.Dashboards.Tests
{
    public partial class TestForm : Form
    {
        private bool gridShown;

        public TestForm()
        {
            InitializeComponent();

            gridShown = false;
        }

        private void BtnChangeGrid_Click(object sender, EventArgs e)
        {
            if (gridShown)
                dashboard.ClearLines();
            else
                dashboard.DrawLines();

            gridShown = !gridShown;
        }
    }
}
