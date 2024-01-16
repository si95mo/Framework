using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tasks;

namespace UserInterface.Controls
{
    public partial class TaskControl : UserControl
    {
        private const int BufferSize = 8;

        private readonly Size notExpandedSize = new Size(1144, 32);
        private readonly Size expandedSize = new Size(1144, 232);

        private IAwaitable task;
        private int bufferIndex;
        private Label[] taskWaitStateLabels;

        public TaskControl(IAwaitable task)
        {
            InitializeComponent();

            btnStart.TabStop = false;
            btnStop.TabStop = false;

            lblTaskCode.Text = task.Code;
            lblTaskStatus.Text = task.Status.ToString();
            lblTaskWaitState.Text = task.WaitState.ToString();

            this.task = task;
            taskWaitStateLabels = new Label[BufferSize];

            // Create an array for the wait state labels (the designer labels are only for visualization, the is done through the array)
            for (int i = 0; i < BufferSize; i++)
            {
                if (i == 0)
                {
                    taskWaitStateLabels[i] = lblWaitState1;
                }
                else if (i == 1)
                {
                    taskWaitStateLabels[i] = lblWaitState2;
                }
                else if (i == 2)
                {
                    taskWaitStateLabels[i] = lblWaitState3;
                }
                else if (i == 3)
                {
                    taskWaitStateLabels[i] = lblWaitState4;
                }
                else if (i == 4)
                {
                    taskWaitStateLabels[i] = lblWaitState5;
                }
                else if (i == 5)
                {
                    taskWaitStateLabels[i] = lblWaitState6;
                }
                else if (i == 6)
                {
                    taskWaitStateLabels[i] = lblWaitState7;
                }
                else if (i == 7)
                {
                    taskWaitStateLabels[i] = lblWaitState8;
                }

                taskWaitStateLabels[i].Text = string.Empty;
            }

            bufferIndex = 0;

            task.Status.ValueChanged += TaskStatus_ValueChanged;
            task.WaitState.ValueChanged += WaitState_ValueChanged;

            lblTaskCode.DoubleClick += Control_DoubleClick;
            lblTaskWaitState.DoubleClick += Control_DoubleClick;
            lblTaskStatus.DoubleClick += Control_DoubleClick;

            Size = notExpandedSize;
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

                    bufferIndex = 0;
                    for (int i = 0; i < BufferSize; i++)
                    {
                        taskWaitStateLabels[i].Text = string.Empty;
                    }
                }
                else if (task.Status.Value == TaskStatus.WaitingToRun || task.Status.Value == TaskStatus.Running)
                {
                    btnStart.Enabled = false;
                    btnStop.Enabled = true;
                }
            }
            else
            {
                BeginInvoke(new Action(() => TaskStatus_ValueChanged(sender, e)));
            }
        }

        private void WaitState_ValueChanged(object sender, Core.ValueChangedEventArgs e)
        {
            if (!InvokeRequired)
            {
                string waitState = e.NewValueAsString;

                lblTaskWaitState.Text = waitState;

                if (Size == expandedSize && (bufferIndex + 1) % BufferSize == 0) // Clean buffers only if the control is actually expanded
                {
                    for (int i = 0; i < BufferSize; i++)
                    {
                        taskWaitStateLabels[i].Text = string.Empty;
                    }
                }

                string waitStateWithInfo = $"{DateTime.Now:HH:mm:ss.fff} → {waitState}";
                taskWaitStateLabels[++bufferIndex % BufferSize].Text = waitStateWithInfo; // Increment counter here

                if (bufferIndex % BufferSize == 0)
                {
                    for (int i = 1; i < BufferSize; i++)
                    {
                        taskWaitStateLabels[i].Text = string.Empty;
                    }
                }
            }
            else
            {
                BeginInvoke(new Action(() => WaitState_ValueChanged(sender, e)));
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            task.Start();
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            task.Stop();
        }

        private void PbxDetails_Click(object sender, EventArgs e)
        {
            if (!InvokeRequired)
            {
                if (Size == notExpandedSize) // Not expanded, so expand control and show wait states
                {
                    pbxDetails.Image = Properties.Resources.ChevronUp;
                    Size = expandedSize;
                }
                else // Expanded, so shrink control and hide details
                {
                    pbxDetails.Image = Properties.Resources.ChevronDown;
                    Size = notExpandedSize;
                }
            }
            else
            {
                BeginInvoke(new Action(() => PbxDetails_Click(sender, e)));
            }
        }

        private void Control_DoubleClick(object sender, EventArgs e)
        {
            PbxDetails_Click(this, new EventArgs());
        }
    }
}