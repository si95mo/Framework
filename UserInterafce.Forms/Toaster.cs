using System;
using System.Drawing;
using System.Windows.Forms;
using UserInterface.Controls;

namespace UserInterface.Forms
{
    internal partial class Toaster : Form
    {
        public Toaster()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.Manual;
            TopMost = true;

            // Adjust labels Z-value to avoid overlapping in the close label
            lblClose.BringToFront();
            lblMessage.SendToBack();
        }

        #region Public methods

        /// <summary>
        /// Show the <see cref="Toaster"/>
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="toasterType">The <see cref="ToasterType"/></param>
        /// <param name="owner">The <see cref="Toaster"/> owner (i.e. who calls the <see cref="Form.ShowDialog()"/></param>
        /// <param name="displayDurationInMilliseconds">The <see cref="Toaster"/> display duration, in milliseconds</param>
        public void ShowToaster(string message, ToasterType toasterType, Form owner, int displayDurationInMilliseconds = 10000)
        {
            lblMessage.Text = message;

            switch (toasterType)
            {
                case ToasterType.Message:
                    BackColor = Colors.LightBlue;
                    break;

                case ToasterType.Warning:
                    BackColor = Colors.LightYellow;
                    break;

                case ToasterType.Error:
                    BackColor = Colors.Red;
                    break;
            }

            Size ownerSize = owner.Size;
            Point ownerLocation = owner.Location;

            // Center the control based on the owner that called it
            int x = (ownerSize.Width - Width) / 2;
            int y = ownerLocation.Y + UiService.ToasterCounter == 1 ? UiService.ToasterCounter * Height + Height / 2 : UiService.ToasterCounter * (Height + 8) + Height / 2;
            Location = new Point(x, y);

            Show();

            // Start the timer
            timer.Interval = displayDurationInMilliseconds;
            timer.Enabled = true;
            timer.Start();
        }

        /// <summary>
        /// Show the <see cref="Toaster"/>
        /// </summary>
        /// <param name="toaster">The <see cref="Toaster"/> to use</param>
        /// <param name="message">The message to display</param>
        /// <param name="toasterType">The <see cref="ToasterType"/></param>
        /// <param name="owner">The <see cref="Toaster"/> owner (i.e. who calls the <see cref="Form.ShowDialog()"/></param>
        /// <param name="displayDurationInMilliseconds">The <see cref="Toaster"/> display duration, in milliseconds</param>
        public static void ShowToaster(Toaster toaster, string message, ToasterType toasterType, Form owner, int displayDurationInMilliseconds = 10000)
            => toaster.ShowToaster(message, toasterType, owner, displayDurationInMilliseconds);

        #endregion Public methods

        #region Event handlers

        private void LblClose_Click(object sender, EventArgs e)
        {
            DestroyToaster();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!InvokeRequired)
            {
                DestroyToaster();
            }
            else
            {
                BeginInvoke(new Action(() => DestroyToaster()));
            }
        }

        #endregion Event handlers

        #region Private methods

        /// <summary>
        /// Destroy the <see cref="Toaster"/> (i.e. <see cref="Form.Close"/> and <see cref="Form.Dispose(bool)"/> the window)
        /// </summary>
        private void DestroyToaster()
        {
            timer.Stop();
            timer.Enabled = false;

            Close();
            Dispose();
        }

        #endregion Private methods
    }
}