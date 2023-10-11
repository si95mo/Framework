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

            timer.Interval = 10000; // 10s
            timer.Enabled = true;
            timer.Start();
        }

        #region Public methods

        /// <summary>
        /// Show the <see cref="Toaster"/>
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="toasterType">The <see cref="ToasterType"/></param>
        /// <param name="owner">The <see cref="Toaster"/> owner (i.e. who calls the <see cref="Form.ShowDialog()"/></param>
        public void ShowToaster(string message, ToasterType toasterType, Form owner)
        {
            lblMessage.Text = message;

            switch(toasterType)
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
            Location = new Point((ownerSize.Width - Width) / 2, ownerLocation.Y - (Height + Height / 2));

            ShowDialog(owner);
        }

        /// <summary>
        /// Show the <see cref="Toaster"/>
        /// </summary>
        /// <param name="toaster">The <see cref="Toaster"/> to use</param>
        /// <param name="message">The message to display</param>
        /// <param name="toasterType">The <see cref="ToasterType"/></param>
        /// <param name="owner">The <see cref="Toaster"/> owner (i.e. who calls the <see cref="Form.ShowDialog()"/></param>
        public static void ShowToaster(Toaster toaster, string message, ToasterType toasterType, Form owner)
            => toaster.ShowToaster(message, toasterType, owner);

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
                DestroyHandle();
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
