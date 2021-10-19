using System;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    public partial class CustomForm : Form
    {
        public CustomForm()
        {
            InitializeComponent();
        }

        // Mouse drag variables
        public const int WM_NCLBUTTONDOWN = 0xA1;

        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        /// <summary>
        /// Handle the mouse down event (for both form and top panel)
        /// in order to make the application draggable
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="MouseEventArgs"/></param>
        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            lblFormName.Text = Text;
        }
    }
}