using System;
using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    public partial class DigitalControl : BaseControl
    {
        private readonly Color onColor = Color.LightGreen;
        private readonly Color offColor = Color.DarkSlateGray;

        public override object Value
        {
            get => value;
            set => this.value = (bool)value;
        }

        public DigitalControl()
        {
            InitializeComponent();

            btnValue.FlatAppearance.BorderColor = offColor;
        }

        private void ButtonValue_Click(object sender, EventArgs e)
        {
            Point p;
            Color c;
            string text;

            if (btnValue.Location.X == 0)
            {
                p = new Point(75, 0);
                text = "True";
                c = onColor;

                value = true;
            }
            else
            {
                p = new Point(0, 0);
                text = "False";
                c = offColor;

                value = false;
            }

            btnValue.Location = p;
            btnValue.FlatAppearance.BorderColor = c;
            btnValue.Text = text;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
    }
}
