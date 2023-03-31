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
        private string[] buffer; // Wait states buffer
        private int bufferIndex;
        private Label[] waitStates;

        public TaskControl(IAwaitable task)
        {
            InitializeComponent();

            btnStart.TabStop = false;
            btnStop.TabStop = false;

            lblTaskCode.Text = task.Code;
            lblTaskStatus.Text = task.Status.ToString();
            lblTaskWaitState.Text = task.WaitState.ToString();

            this.task = task;
            buffer = new string[BufferSize];
            waitStates = new Label[BufferSize];

            // Create an array for the wait state labels (the designer labels are only for visualization, the is done through the array)
            for (int i = 0; i < BufferSize; i++)
            {
                if (i == 0)
                    waitStates[i] = lblWaitState1;
                else if(i == 1)
                    waitStates[i] = lblWaitState2;
                else if (i == 2)
                    waitStates[i] = lblWaitState3;
                else if (i == 3)
                    waitStates[i] = lblWaitState4;
                else if (i == 4)
                    waitStates[i] = lblWaitState5;
                else if (i == 5)
                    waitStates[i] = lblWaitState6;
                else if (i == 6)
                    waitStates[i] = lblWaitState7;
                else if (i == 7)
                    waitStates[i] = lblWaitState8;

                waitStates[i].Text = string.Empty;
            }

            bufferIndex = 0;

            task.Status.ValueChanged += TaskStatus_ValueChanged;
            task.WaitState.ValueChanged += WaitState_ValueChanged;

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
            if (!InvokeRequired)
            {
                string waitState = e.NewValueAsString;

                lblTaskWaitState.Text = waitState;

                if(Size == expandedSize && bufferIndex == 0) // Clean buffers only if the control is actually expanded
                {
                    for(int i = 0; i < BufferSize; i++)
                    {
                        buffer[i] = string.Empty;
                        waitStates[i].Text = string.Empty;
                    }
                }

                string waitStateWithInfos = $"({DateTime.Now:HH:mm:ss.fff}), {waitState}";
                buffer[bufferIndex % BufferSize] = waitStateWithInfos;
                waitStates[bufferIndex % BufferSize].Text = waitStateWithInfos; // Increment counter here

                if(bufferIndex++ == BufferSize) // Reset index and clean the buffers
                    bufferIndex = 0;
            }
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
                BeginInvoke(new Action(() => PbxDetails_Click(sender, e)));
        }
    }
}
