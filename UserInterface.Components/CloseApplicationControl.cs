using System;

namespace UserInterface.Controls
{
    public partial class CloseApplicationControl : BaseControl
    {
        private bool dialogOpen;
        private DialogPanel dialogPanel;

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