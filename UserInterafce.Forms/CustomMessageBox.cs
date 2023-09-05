using System;
using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Forms
{
    /// <summary>
    /// Implement a custom message box compliant with the
    /// user interface general style
    /// </summary>
    public partial class CustomMessageBox : CustomForm
    {
        /// <summary>
        /// Create a new instance of <see cref="CustomMessageBox"/>
        /// </summary>
        /// <param name="container">The <see cref="Form"/> container (i.e. the parent)</param>
        /// <param name="title">The title</param>
        /// <param name="message">The message</param>
        protected CustomMessageBox(Form container, string title, string message) : base()
        {
            InitializeComponent();

            ShowInTaskbar = false;
            Owner = container;

            Size = new Size(container.Size.Width, Size.Height);
            Location = new Point(container.Location.X, (container.Location.Y + Size.Height / 2));
            StartPosition = FormStartPosition.Manual;

            Text = title;
            lblMessage.Text = message;

            int labelVerticalLocation = (Size.Height - lblMessage.Size.Height) / 2;
            lblMessage.Location = new Point(lblFormName.Location.X, labelVerticalLocation);

            controlBox.Visible = false;
            controlBox.Enabled = false;

            MouseDown -= Form_MouseDown;
            FormClosing -= CustomForm_FormClosing;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }

        /// <summary>
        /// Show the <see cref="CustomMessageBox"/>
        /// (in a modal way)
        /// </summary>
        /// <param name="container">The <see cref="Form"/> container (i.e. parent)</param>
        /// <param name="title">The title</param>
        /// <param name="message">The message</param>
        public static void Show(Form container, string title, string message)
            => (new CustomMessageBox(container, title, message)).ShowDialog();
    }
}