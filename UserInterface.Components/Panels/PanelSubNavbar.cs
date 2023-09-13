using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.Controls.Panels
{
    public partial class PanelSubNavbar : PanelNavbar
    {
        public PanelSubNavbar() : base()
        {
            InitializeComponent();

            LblVersion.Visible = false;

            LayoutSize = Size;
            LayoutPanel.Location = new Point(0, 0);
            LayoutPanel.BackColor = Colors.Grey;

            PbxClose.Enabled = false;
            PbxClose.Visible = false;
            PbxClose.SendToBack();

            PbxLogo.Enabled = false;
            PbxLogo.Visible = false;
            PbxLogo.SendToBack();

            AutoScaleMode = AutoScaleMode.Inherit;

            Load += PanelSubNavbar_Load;
        }

        private void PanelSubNavbar_Load(object sender, EventArgs e)
        {
            Task task = new Task(async () =>
                {
                    while (true)
                    {
                        if (!InvokeRequired)
                            UpdateDateAndTime(DateTime.Now);
                        else
                            BeginInvoke(new Action(() => UpdateDateAndTime(DateTime.Now)));

                        await Task.Delay(500); // Wait for 500ms, maximum time to not lose time information
                    }
                }
            );
            task.Start();
        }

        private void UpdateDateAndTime(DateTime dateTime)
        {
            lblTime.Text = dateTime.ToString("HH:mm:ss");
            lblDate.Text = dateTime.ToString("yyyy/MM/dd");
        }
    }
}
