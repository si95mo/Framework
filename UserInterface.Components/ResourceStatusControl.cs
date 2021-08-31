using Core;
using Hardware;
using System;
using System.Drawing;
using System.Windows.Forms;
using UserInterface.Controls.Properties;

namespace UserInterface.Controls
{
    /// <summary>
    /// Implement a control to handle the resource status in the
    /// user interface
    /// </summary>
    public partial class ResourceStatusControl : BaseControl
    {
        private IResource resource;
        private bool eventEnabled;

        /// <summary>
        /// The <see cref="StatusChangedEventArgs"/>
        /// handler enabling
        /// </summary>
        public bool EventEnabled { get => eventEnabled; set => eventEnabled = value; }

        /// <summary>
        /// Create a new instance of <see cref="ResourceStatusControl"/>
        /// </summary>
        private ResourceStatusControl()
        {
            InitializeComponent();

            btnStart.Text = "";
            btnStart.Image = Resources.ImageStart;
            btnStart.BackColor = Colors.Transparent;

            eventEnabled = false;
        }

        /// <summary>
        /// Create a new instance of <see cref="ResourceStatusControl"/>
        /// </summary>
        /// <param name="resource">The <see cref="IResource"/></param>
        public ResourceStatusControl(IResource resource) : this()
        {
            this.resource = resource;
            this.resource.StatusChanged += Resource_StatusChanged;

            if (resource.Status == ResourceStatus.Executing || resource.Status == ResourceStatus.Starting)
                btnStart.Image = Resources.ImageStop;
            else
                btnStart.Image = Resources.ImageStart;

            UpdateUserInterface();
        }

        private void Resource_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            if (eventEnabled)
                UpdateUserInterface();
        }

        /// <summary>
        /// Update the <see cref="Image"/>
        /// shown in the start <see cref="Button"/>
        /// </summary>
        private void UpdateButtonImage()
        {
            Image img;

            if (resource.Status == ResourceStatus.Executing || resource.Status == ResourceStatus.Starting)
                img = Resources.ImageStop;
            else
                img = Resources.ImageStart;

            btnStart.Invoke(new MethodInvoker(() => btnStart.Image = img));
        }

        /// <summary>
        /// Update the <see cref="ResourceStatusControl"/>
        /// based on the <see cref="IResource"/> <see cref="IResource.Status"/>
        /// </summary>
        private void UpdateUserInterface()
        {
            string code = resource.Code;
            ResourceStatus status = resource.Status;
            IFailure failure = resource.LastFailure;

            try
            {
                Invoke(new MethodInvoker(() =>
                        BackColor = status == ResourceStatus.Failure ?
                            ControlPaint.LightLight(Colors.Error) : ControlPaint.LightLight(Colors.BackgroundColor)
                    )
                );

                lblCode.Invoke(new MethodInvoker(() =>
                        lblCode.Text = code
                    )
                );
                lblStatus.Invoke(new MethodInvoker(() =>
                        lblStatus.Text = status.ToString()
                    )
                );
                lblFailure.Invoke(new MethodInvoker(() =>
                        lblFailure.Text = failure.Description
                    )
                );
                lblCode.Invoke(new MethodInvoker(() =>
                        lblTimestamp.Text = failure.Timestamp.ToString("yyyy/MM/dd - HH:mm:ss")
                    )
                );
            }
            catch (Exception ex)
            {
                BackColor = status == ResourceStatus.Failure ?
                            ControlPaint.LightLight(Colors.Error) : ControlPaint.LightLight(Colors.BackgroundColor);

                lblCode.Text = code;
                lblStatus.Text = status.ToString();
                lblFailure.Text = failure.Description;
                lblTimestamp.Text = failure.Timestamp.ToString("yyyy/MM/dd - HH:mm:ss");
            }
        }

        private async void BtnStart_Click(object sender, EventArgs e)
        {
            if (resource.Status != ResourceStatus.Executing && resource.Status != ResourceStatus.Starting)
                await resource.Start();
            else
                resource.Stop();

            UpdateUserInterface();
            UpdateButtonImage();
        }
    }
}