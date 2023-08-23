using System.Windows.Forms;

namespace UserInterface.Controls.Panels
{
    public partial class PanelWithNavbar : UserControl
    {
        public PanelWithNavbar()
        {
            InitializeComponent();
            PanelNavbar.ActualUserControlChanged += PanelNavbar_ActualUserControlChanged;
        }

        public void AddControl(string text, UserControl control)
            => PanelNavbar.Add(text, control);

        private void PanelNavbar_ActualUserControlChanged(object source, ActualUserControlEventArgs e)
        {
            panelContainer.Controls.Clear();
            panelContainer.Controls.Add(e.Control);
        }
    }
}
