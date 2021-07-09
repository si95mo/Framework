using System;
using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    /// <summary>
    /// Define a control for handling digital inputs.
    /// See <see cref="BaseControl"/>
    /// </summary>
    public partial class DigitalControl : BaseControl
    {
        /// <summary>
        /// The <see cref="DigitalControl"/> value
        /// </summary>
        public override object Value
        {
            get => (bool)value;
            set => this.value = (bool)value;
        }

        /// <summary>
        /// Create a new instance of <see cref="DigitalControl"/>
        /// </summary>
        public DigitalControl()
        {
            InitializeComponent();

            value = false;

            btnValue.FlatAppearance.BorderColor = Colors.Black;
            btnValue.FlatAppearance.MouseOverBackColor = Colors.Transparent;
        }

        /// <summary>
        /// Handle the click event
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="EventArgs"/></param>
        private void Control_Cick(object sender, EventArgs e)
        {
            Point p;
            string text;

            if (btnValue.Location.X == 0)
            {
                p = new Point(75, 0);
                text = "True";
                btnValue.FlatAppearance.BorderColor = Colors.Green;
            }
            else
            {
                p = new Point(0, 0);
                text = "False";
                btnValue.FlatAppearance.BorderColor = Colors.Black;
            }

            value = !(bool)value;

            btnValue.Location = p;
            btnValue.Text = text;
        }

        /// <summary>
        /// The on paint event handler
        /// </summary>
        /// <param name="e">THe <see cref="PaintEventArgs"/></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
    }
}