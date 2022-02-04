using System;
using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    /// <summary>
    /// Implement a visual form with a predefined user interface style.
    /// See also <see cref="Form"/>
    /// </summary>
    public partial class CustomForm : Form
    {
        /// <summary>
        /// Create a new instance of <see cref="CustomForm"/>
        /// </summary>
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
        protected void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        /// <summary>
        /// Handle the on load event (set the form name and resize inherited components)
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="EventArgs"/></param>
        private void Form_Load(object sender, EventArgs e)
        {
            lblFormName.Text = Text;
            controlBox.Location = new Point(
                Size.Width - controlBox.Size.Width - 4,
                controlBox.Location.Y
            );
            borderUpPanel.Size = new Size(Size.Width + 2, borderUpPanel.Size.Height);
        }

        /// <summary>
        /// Show an alert message
        /// </summary>
        /// <param name="title">The title</param>
        /// <param name="message">The message</param>
        protected void ShowAlert(string title = "Attention", string message = "Attention message")
            => CustomMessageBox.Show(this, title, message);
    }
}