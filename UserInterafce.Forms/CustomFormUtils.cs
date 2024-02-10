using System;
using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Forms
{
    public partial class CustomForm
    {
        #region Constants

        public const int PanelWithNavbarX = 0;
        public const int PanelWithNavbarY = 60;

        #endregion Constants

        #region Mouse drag

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

        #endregion Mouse drag

        #region Event handlers

        /// <summary>
        /// Handle the on load event (set the form name and resize inherited components)
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="EventArgs"/></param>
        private void Form_Load(object sender, EventArgs e)
        {
            LblFormName.Text = Text;
            ControlBox.Location = new Point(Size.Width - ControlBox.Size.Width - 4, ControlBox.Location.Y);
            borderUpPanel.Size = new Size(Size.Width + 2, borderUpPanel.Size.Height);
        }

        /// <summary>
        /// Handle the closing event (confirmation)
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/></param>
        protected void CustomForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!(sender is CustomMessageBox) && !(sender is CustomQuestionBox) && !invokedFromThis)
            {
                DialogResult result = AskQuestion("Attention", "Do you really want to exit?"); // Return OK or Cancel
                if (result == DialogResult.No || result == DialogResult.Cancel) // Add No for future upgrades
                {
                    TopMost = true;

                    e.Cancel = true;
                    invokedFromThis = false;
                }
                else
                {
                    invokedFromThis = true;

                    Close();
                    Dispose();

                    Application.Exit(); // Force exit
                }

                TopMost = false;
            }
        }

        #endregion Event handlers
    }
}
