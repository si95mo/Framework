using System;
using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Dashboards
{
    /// <summary>
    /// Implement a <see cref="DraggableControl"/> that can
    /// be resized on runtime
    /// </summary>
    public partial class ResizableControl : DraggableControl
    {
        private const int Grab = 16;

        /// <summary>
        /// Create a new instance of <see cref="ResizableControl"/>
        /// </summary>
        protected ResizableControl() : base()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Rectangle rectangle = new Rectangle(ClientSize.Width - Grab, ClientSize.Height - Grab, Grab, Grab);
            ControlPaint.DrawSizeGrip(e.Graphics, BackColor, rectangle);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x84) // Trap WM_NCHITTEST
            {
                var pos = PointToClient(new Point(m.LParam.ToInt32()));
                if (pos.X >= ClientSize.Width - Grab && pos.Y >= ClientSize.Height - Grab)
                    m.Result = new IntPtr(17);  // HT_BOTTOMRIGHT
            }
        }
    }
}