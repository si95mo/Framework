using System;
using System.Drawing;
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

        protected override void OnPaint(PaintEventArgs e)
        {
            // Round corners
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            base.OnPaint(e);
        }
    }
}
