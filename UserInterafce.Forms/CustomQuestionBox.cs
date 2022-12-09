using System;
using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Forms
{
    /// <summary>
    /// Implement a custom question message box compliant with the
    /// user interface general style
    /// </summary>
    public partial class CustomQuestionBox : CustomForm
    {
        /// <summary>
        /// Create a new instance of <see cref="CustomQuestionBox"/>
        /// </summary>
        /// <param name="container">The <see cref="Form"/> container (i.e. the parent)</param>
        /// <param name="title">The title</param>
        /// <param name="message">The message</param>
        public CustomQuestionBox(Form container, string title, string message) : base()
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
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
            Dispose();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            Close();
            Dispose();
        }

        /// <summary>
        /// Show the <see cref="CustomQuestionBox"/>
        /// (in a modal way)
        /// </summary>
        /// <param name="container">The <see cref="Form"/> container (i.e. parent)</param>
        /// <param name="title">The title</param>
        /// <param name="message">The message</param>
        /// <returns>The <see cref="DialogResult"/></returns>
        public static DialogResult Show(Form container, string title, string message)
            => (new CustomQuestionBox(container, title, message)).ShowDialog();
    }
}
