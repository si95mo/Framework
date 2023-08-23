using System.Windows.Forms;

namespace UserInterface.Controls.Panels
{
    public partial class PanelSubNavbar : PanelNavbar
    {
        public PanelSubNavbar() : base()
        {
            InitializeComponent();

            LayoutSize = Size;
            LayoutPanel.BackColor = Colors.Grey;

            PbxClose.Enabled = false;
            PbxClose.Visible = false;
            PbxClose.SendToBack();

            PbxLogo.Enabled = false;
            PbxLogo.Visible = false;
            PbxLogo.SendToBack();

            AutoScaleMode = AutoScaleMode.Inherit;
        }
    }
}
