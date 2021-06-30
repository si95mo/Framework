using System;
using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    public partial class DigitalControl : BaseControl
    {     
        public override object Value
        {
            get => (bool)value;
            set => this.value = (bool)value;
        }

        public DigitalControl()
        {
            InitializeComponent();

            value = false;

            // panel.BackColor = Colors.BackgroundColor;

            // btnValue.BackColor = Colors.BackgroundColor;
            // btnValue.ForeColor = Colors.TextColorLight;
            btnValue.FlatAppearance.BorderColor = Colors.OffColor;            
            btnValue.FlatAppearance.MouseOverBackColor = Colors.Transparent;            
        }

        private void Control_Cick(object sender, EventArgs e)
        {
            Point p;
            string text;

            if (btnValue.Location.X == 0)
            {
                p = new Point(75, 0);
                text = "True";
                btnValue.FlatAppearance.BorderColor = Colors.OnColor;
            }
            else
            {
                p = new Point(0, 0);
                text = "False";
                btnValue.FlatAppearance.BorderColor = Colors.OffColor;
            }

            value = !(bool)value;

            btnValue.Location = p;
            btnValue.Text = text;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
    }
}
