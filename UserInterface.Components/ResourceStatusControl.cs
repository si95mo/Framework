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
        /// The <see cref="EventEnabled"/> handler
        /// </summary>
        public bool EventEnabled
        {
            get => eventEnabled;
            set
            {
                eventEnabled = value;
                UpdateUserInterface();
            }
        }

        /// <summary>
        /// Create a new instance of <see cref="ResourceStatusControl"/>
        /// </summary>
        private ResourceStatusControl()
        {
            InitializeComponent();

            btnStart.Text = "";
            btnStart.Image = Resources.ImageStart;
            btnStart.BackColor = Colors.Transparent;
            btnStart.FlatAppearance.MouseOverBackColor = Color.Transparent;

            eventEnabled = false;
        }

        /// <summary>
        /// Create a new instance of <see cref="ResourceStatusControl"/>
        /// </summary>
        /// <param name="resource">The <see cref="IResource"/></param>
        public ResourceStatusControl(IResource resource) : this()
        {
            this.resource = resource;
            this.resource.Status.ValueChanged += Status_ValueChanged;

            if (resource.Status.Value == ResourceStatus.Executing || resource.Status.Value == ResourceStatus.Starting)
                btnStart.Image = Resources.ImageStop;
            else
                btnStart.Image = Resources.ImageStart;

            UpdateUserInterface();
        }

        private void Status_ValueChanged(object sender, ValueChangedEventArgs e)
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

            if (resource.Status.Value == ResourceStatus.Executing || resource.Status.Value == ResourceStatus.Starting)
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
            try
            {
                string code = resource.Code;
                ResourceStatus status = resource.Status.Value;
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
                catch (Exception)
                {
                    BackColor = status == ResourceStatus.Failure ?
                                ControlPaint.LightLight(Colors.Error) : ControlPaint.LightLight(Colors.BackgroundColor);

                    lblCode.Text = code;
                    lblStatus.Text = status.ToString();
                    lblFailure.Text = failure.Description;
                    lblTimestamp.Text = failure.Timestamp.ToString("yyyy/MM/dd - HH:mm:ss");
                }
            }
            catch (Exception)
            { }
        }

        private async void BtnStart_Click(object sender, EventArgs e)
        {
            if (resource.Status.Value != ResourceStatus.Executing && resource.Status.Value != ResourceStatus.Starting)
                await resource.Start();
            else
                resource.Stop();

            UpdateUserInterface();
            UpdateButtonImage();
        }
    }
}