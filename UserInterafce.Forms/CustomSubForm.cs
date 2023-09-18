using System;
using System.Drawing;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using UserInterface.Controls;

namespace UserInterface.Forms
{
    /// <summary>
    /// Define a <see cref="CustomForm"/> that can be used as a sub form
    /// </summary>
    public partial class CustomSubForm : CustomForm
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        ); 

        /// <summary>
        /// Create a new instance of <see cref="CustomSubForm"/>
        /// </summary>
        public CustomSubForm() : base()
        {
            InitializeComponent();
            BackColor = Color.White;

            // Remove closing event handler, a close of this form should not invoke the parent event handler
            FormClosing -= CustomForm_FormClosing;
        }

        /// <summary>
        /// <see cref="Control.Show())"/> this form with default appearance
        /// </summary>
        /// <param name="text">The <see cref="Text"/> to show</param>
        /// <param name="opacity">The opacity of the <see cref="CustomSubForm"/></param>
        public void ShowAsModalForm(string text = "", double opacity = 0.9)
        {
            AutoScaleMode = AutoScaleMode.Inherit;
            TopMost = true;
            Opacity = opacity;
            Text = text;
            Location = new Point((1920 - Size.Width) / 2, (840 - Size.Height) / 2);

            Show();
        }

        /// <summary>
        /// <see cref="Form.ShowDialog())"/> this form with default appearance
        /// </summary>
        /// <param name="text">The <see cref="Text"/> to show</param>
        /// <param name="opacity">The opacity of the <see cref="CustomSubForm"/></param>
        public DialogResult ShowAsExclusiveModalForm(string text = "", double opacity = 0.9)
        {
            AutoScaleMode = AutoScaleMode.Inherit;
            TopMost = true;
            Opacity = opacity;
            Text = FindForm().Text + text;
            Location = new Point((1920 - Size.Width) / 2, (840 - Size.Height) / 2);

            return ShowDialog();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Round corners
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            base.OnPaint(e);
        }
    }
}
