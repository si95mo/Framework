using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    public class LabelControl : Label
    {
        public LabelControl() : base()
        {
            Font = new Font("Lucida Sans Unicode", 12);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            //
            // LabelControl
            //
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.ResumeLayout(false);
        }
    }
}