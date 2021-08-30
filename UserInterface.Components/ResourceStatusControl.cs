using Core;
using Hardware;
using System;
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

        /// <summary>
        /// Create a new instance of <see cref="ResourceStatusControl"/>
        /// </summary>
        private ResourceStatusControl()
        {
            InitializeComponent();

            btnStart.Text = "";
            btnStart.Image = Resources.ImageStart;
            btnStart.BackColor = Colors.Transparent;
        }

        /// <summary>
        /// Create a new instance of <see cref="ResourceStatusControl"/>
        /// </summary>
        /// <param name="resource">The <see cref="IResource"/></param>
        public ResourceStatusControl(IResource resource) : this()
        {
            this.resource = resource;
            this.resource.StatusChanged += Resource_StatusChanged;

            UpdateUserInterface();
        }

        private void Resource_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            UpdateUserInterface();
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
            catch(Exception)
            {
                BackColor = status == ResourceStatus.Failure ?
                            ControlPaint.LightLight(Colors.Error) : ControlPaint.LightLight(Colors.BackgroundColor);

                lblCode.Text = code;
                lblStatus.Text = status.ToString();
                lblFailure.Text = failure.Description;
                lblTimestamp.Text = failure.Timestamp.ToString("yyyy/MM/dd - HH:mm:ss");
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            resource.Start();
            UpdateUserInterface();
        }
    }
}
