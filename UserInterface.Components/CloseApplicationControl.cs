using System;

namespace UserInterface.Controls
{
    /// <summary>
    /// Implement a control for closing an application
    /// </summary>
    public partial class CloseApplicationControl : BaseControl
    {
        private bool dialogOpen;
        private DialogPanel dialogPanel;

        /// <summary>
        /// Create a new instance of <see cref="CloseApplicationControl"/>
        /// </summary>
        public CloseApplicationControl()
        {
            InitializeComponent();
            dialogOpen = false;
        }

        private void CloseImage_Click(object sender, EventArgs e)
        {
            if (!dialogOpen)
            {
                dialogPanel = new DialogPanel(this);
                dialogOpen = true;
            }
            else
            {
                dialogPanel.Dispose();
                dialogOpen = false;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
        }
    }
}