using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using UserInterface.Controls;

namespace UserInterface.Forms
{
    /// <summary>
    /// Implement a visual form with a predefined user interface style.
    /// See also <see cref="Form"/>
    /// </summary>
    public partial class CustomForm : Form
    {
        public const int PanelWithNavbarX = 0;
        public const int PanelWithNavbarY = 60;

        [Browsable(true)]
        [Category("Behavior")]
        public bool FullSize { get; set; } = true;

        private bool invokedFromThis;

        /// <summary>
        /// Create a new instance of <see cref="CustomForm"/>
        /// </summary>
        public CustomForm()
        {
            InitializeComponent();

            if(FullSize)
            {
                Rectangle workingArea = Screen.FromHandle(Handle).WorkingArea;
                MaximizedBounds = new Rectangle(0, 0, workingArea.Width, workingArea.Height);
                WindowState = FormWindowState.Maximized;
            }

            invokedFromThis = false;
        }

        /// <summary>
        /// Create a new instance of <see cref="CustomForm"/> with the option to automatically set its dimensions to full size
        /// </summary>
        /// <param name="fullSize">The full size option</param>
        public CustomForm(bool fullSize) : this()
        {
            FullSize = fullSize;
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
            LblFormName.Text = Text;
            ControlBox.Location = new Point(Size.Width - ControlBox.Size.Width - 4, ControlBox.Location.Y);
            borderUpPanel.Size = new Size(Size.Width + 2, borderUpPanel.Size.Height);
        }

        /// <summary>
        /// Adjust the <see cref="CustomForm"/> size. Should be called inside the constructor of the derived classes
        /// </summary>
        protected void AdjustSize()
        {
            Rectangle workingArea = Screen.FromHandle(Handle).WorkingArea;

            MaximizedBounds = new Rectangle(0, 0, workingArea.Width, workingArea.Height);
            WindowState = FormWindowState.Maximized;
        }

        /// <summary>
        /// Show an alert message
        /// </summary>
        /// <remarks>
        /// This method call <see cref="CustomMessageBox.Show(Form, string, string)"/> and pass <see langword="this"/> in the first parameter
        /// </remarks>
        /// <param name="title">The title</param>
        /// <param name="message">The message</param>
        public void ShowAlert(string title = "Attention", string message = "Attention message")
            => CustomMessageBox.Show(this, title, message);

        /// <summary>
        /// Show a question
        /// </summary>
        /// <param name="title">The title</param>
        /// <param name="message">The message</param>
        public DialogResult AskQuestion(string title, string message)
            => CustomQuestionBox.Show(this, title, message);

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
    }
}