using System;
using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    internal partial class DialogPanel : BaseControl
    {
        public DialogPanel(BaseControl parent)
        {
            InitializeComponent();

            int x = parent.Location.X - Size.Width + parent.Size.Width;
            int y = parent.Location.Y - Size.Height;

            Location = new Point(x, y);

            parent.Parent.Controls.Add(this);

            BringToFront();
        }

        private void BtnYes_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnNo_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}