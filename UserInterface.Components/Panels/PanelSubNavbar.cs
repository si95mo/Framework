using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.Controls.Panels
{
    public partial class PanelSubNavbar : PanelNavbar
    {
        [Description("Use UTC time"), Category("Data")]
        public bool UseUtcTime { get; set; } = false;

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
                            UpdateDateAndTime();
                        else
                            BeginInvoke(new Action(() => UpdateDateAndTime()));

                        await Task.Delay(30000); // Wait for 30000ms (30s), maximum time to not lose time information displaying only up to minutes
                    }
                }
            );
            task.Start();
        }

        /// <summary>
        /// Update the date and time controls with <see cref="DateTime.Now"/>
        /// </summary>
        private void UpdateDateAndTime()
        {
            DateTime now = UseUtcTime ? DateTime.UtcNow : DateTime.Now;

            lblTime.Text = now.ToString("HH:mm");
            lblDate.Text = now.ToString("yyyy/MM/dd");
        }
    }
}
