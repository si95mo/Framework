using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Forms
{
    /// <summary>
    /// Implement a visual form with a predefined user interface style. Addtional mmethods on partial class inside file <c>"CustomFormUtils"</c>.
    /// See also <see cref="Form"/>
    /// </summary>
    public partial class CustomForm : Form
    {
        [Browsable(true)]
        [Category("Behavior")]
        public bool FullSize { get; set; } = true;

        private bool invokedFromThis;

        /// <summary>
        /// Create a new instance of <see cref="CustomForm"/>
        /// </summary>
        public CustomForm()
        {
            Initialize(FullSize);
        }

        /// <summary>
        /// Create a new instance of <see cref="CustomForm"/> with the option to automatically set its dimensions to full size
        /// </summary>
        /// <param name="fullSize">The full size option</param>
        public CustomForm(bool fullSize)
        {
            Initialize(fullSize);
        }

        #region Public methods

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

        #endregion Public methods

        #region Protected methods

        /// <summary>
        /// Adjust the <see cref="CustomForm"/> size. Should be called inside the constructor of the derived classes
        /// </summary>
        protected void AdjustSize()
        {
            Rectangle workingArea = Screen.FromHandle(Handle).WorkingArea;

            MaximizedBounds = new Rectangle(0, 0, workingArea.Width, workingArea.Height);
            WindowState = FormWindowState.Maximized;
        }

        #endregion Protected methods

        #region Helper methods

        private void Initialize(bool fullSize)
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Inherit;

            if (fullSize)
            {
                Rectangle workingArea = Screen.FromHandle(Handle).WorkingArea;
                MaximizedBounds = new Rectangle(0, 0, workingArea.Width, workingArea.Height);
                WindowState = FormWindowState.Maximized;
            }

            StartPosition = FormStartPosition.CenterScreen;

            FullSize = fullSize;
            invokedFromThis = false;
        }

        #endregion Helper methods
    }
}