using System;
using System.Drawing;
using System.Runtime.InteropServices;
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
            ShowInTaskbar = false;

            // Adjust labels Z-value to avoid overlapping in the close label
            lblClose.BringToFront();
            lblMessage.SendToBack();
            lblMessage.ForeColor = Colors.TextColor;

            InitializeBlur();
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
            Text = message;
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

            MoveToLocation(owner, UiService.ToasterCounter);
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

        #region Internal methods

        /// <summary>
        /// Move the <see cref="Toaster"/> to its default location in the screen, given <see cref="UiService.ToasterCounter"/>
        /// </summary>
        /// <param name="owner">The <see cref="Toaster"/> owner (i.e. who calls the <see cref="Form.ShowDialog()"/></param>
        internal void MoveToLocation(Form owner, int toasterCounter)
        {
            Size ownerSize = owner.Size;
            Point ownerLocation = owner.Location;

            // Center the control based on the owner that called it
            int x = (ownerSize.Width - Width) / 2;
            int y = ownerLocation.Y + toasterCounter == 1 ? toasterCounter * Height + Height / 2 : toasterCounter * (Height + 8) + Height / 2;
            Location = new Point(x, y);
        }

        #endregion Internal methods

        #region Blur effect

        // Import necessary WinAPI functions
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        [StructLayout(LayoutKind.Sequential)]
        private struct AccentPolicy
        {
            public int nAccentState;
            public int nFlags;
            public int nColor;
            public int nAnimationId;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        private enum WindowCompositionAttribute
        {
            WCA_ACCENT_POLICY = 19
        }

        private const int ACCENT_ENABLE_BLURBEHIND = 3;
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_LAYERED = 0x80000;
        private const int WS_EX_TRANSPARENT = 0x20;

        /// <summary>
        /// Initializes the blur effect for the form
        /// </summary>
        private void InitializeBlur()
        {
            // Set form styles to allow transparency and layered rendering
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.Black;
            TransparencyKey = Color.Black;

            int initialStyle = GetWindowLong(this.Handle, GWL_EXSTYLE);
            SetWindowLong(Handle, GWL_EXSTYLE, initialStyle | WS_EX_LAYERED | WS_EX_TRANSPARENT);
            
            EnableBlur(); // Enable blur effect
        }

        /// <summary>
        /// Enables the blur effect for the form using Windows API
        /// </summary>
        private void EnableBlur()
        {
            var accent = new AccentPolicy
            {
                nAccentState = ACCENT_ENABLE_BLURBEHIND,
                nFlags = 2,
                nColor = Color.FromArgb(0x80, BackColor).ToArgb(), // Color for the blur effect (adjust alpha as needed)
                nAnimationId = 0
            };

            var accentStructSize = Marshal.SizeOf(accent);
            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData
            {
                Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };

            SetWindowCompositionAttribute(Handle, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }

        // Windows API functions
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

        #endregion Blur effect
    }
}