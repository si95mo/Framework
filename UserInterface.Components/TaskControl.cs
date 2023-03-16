using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tasks;

namespace UserInterface.Controls
{
    public partial class TaskControl : UserControl
    {
        private IAwaitable task;

        public TaskControl(IAwaitable task)
        {
            InitializeComponent();

            lblTaskCode.Text = task.Code;
            lblTaskStatus.Text = task.Status.ToString();
            lblTaskWaitState.Text = task.WaitState.ToString();

            this.task = task;
            task.Status.ValueChanged += TaskStatus_ValueChanged;
            task.WaitState.ValueChanged += WaitState_ValueChanged;
        }

        private void TaskStatus_ValueChanged(object sender, Core.ValueChangedEventArgs e)
        {
            if (!InvokeRequired)
            {
                lblTaskStatus.Text = e.NewValueAsString;

                // Enable/disable start/stop buttons
                if (task.Status.Value == TaskStatus.RanToCompletion || task.Status.Value == TaskStatus.Canceled || task.Status.Value == TaskStatus.Faulted)
                {
                    btnStart.Enabled = true;
                    btnStop.Enabled = false;
                }
                else if (task.Status.Value == TaskStatus.WaitingToRun || task.Status.Value == TaskStatus.Running)
                {
                    btnStart.Enabled = false;
                    btnStop.Enabled = true;
                }
            }
            else
                BeginInvoke(new Action(() => TaskStatus_ValueChanged(sender, e)));
        }

        private void WaitState_ValueChanged(object sender, Core.ValueChangedEventArgs e)
        {
            if(!InvokeRequired)
                lblTaskWaitState.Text = e.NewValueAsString;
            else
                BeginInvoke(new Action(() => WaitState_ValueChanged(sender, e)));
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            task.Start();
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            task.Stop();
        }
    }
}
