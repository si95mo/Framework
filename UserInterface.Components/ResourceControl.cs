using Hardware;
using System;
using System.Drawing;
using System.Windows.Forms;
using UserInterface.Controls.Properties;

namespace UserInterface.Controls
{
    /// <summary>
    /// Implement an <see cref="UserControl"/> to handle a <see cref="Resource"/>
    /// <see cref="Resource.Start"/> and <see cref="Resource.Stop"/> method
    /// (to modify the <see cref="Resource.Status"/> accordingly)
    /// </summary>
    public partial class ResourceControl : UserControl
    {
        private readonly Color notExecuting = ControlPaint.LightLight(Color.Gold);
        private readonly Color failure = ControlPaint.LightLight(Color.Red);
        private readonly Color notStopped = ControlPaint.LightLight(Color.LightGreen);

        private readonly IResource resource;

        private readonly ToolTip tip;
        private readonly Panel parent;

        private readonly bool initialized = false;

        /// <summary>
        /// Create a new instance of <see cref="ResourceControl"/>
        /// </summary>
        private ResourceControl()
        {
            InitializeComponent();

            btnRestartResource.FlatAppearance.BorderColor = BackColor;
            btnRestartResource.FlatAppearance.MouseOverBackColor = BackColor;
            btnRestartResource.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Create a new instance of <see cref="ResourceControl"/>
        /// </summary>
        /// <remarks>
        /// It's necessary to pass the <paramref name="parent"/> container as a parameter
        /// in order to correctly resize the <see cref="ResourceControl"/>!
        /// </remarks>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="parent">The <see cref="ResourceControl"/> parent <see cref="Panel"/></param>
        public ResourceControl(IResource resource, Panel parent = null) : this()
        {
            this.resource = resource;
            this.parent = parent;

            Load += ResourceControl_Load;

            tip = new ToolTip();
            tip.SetToolTip(lblResourceFailure, lblResourceFailure.Text);
            tip.ToolTipIcon = ToolTipIcon.Info;
            tip.ToolTipTitle = $"{resource.Code} status";
            tip.IsBalloon = true;
            tip.InitialDelay = 100;
            tip.ShowAlways = true;

            initialized = true;
        }

        /// <summary>
        /// Handle the load event
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="EventArgs"/></param>
        private void ResourceControl_Load(object sender, EventArgs e)
        {
            if (parent != null)
            {
                int width = parent.Size.Width - SystemInformation.VerticalScrollBarWidth - 4;
                int heigth = Size.Height;
                Size = new Size(width, heigth);
            }

            UpdateControls();

            resource.Status.ValueChanged += ResourceStatus_ValueChanged;
            btnRestartResource.Click += BtnRestartResource_Click;
        }

        /// <summary>
        /// Handle the <see cref="IResource"/> status value changed event
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="Core.ValueChangedEventArgs"/></param>
        private void ResourceStatus_ValueChanged(object sender, Core.ValueChangedEventArgs e)
        {
            if (!InvokeRequired)
                UpdateControls();
            else
                BeginInvoke(new Action(() => UpdateControls()));
        }

        /// <summary>
        /// Update the UI controls
        /// </summary>
        private void UpdateControls()
        {
            try
            {
                lblResourceCode.Text = resource.Code;
                lblResourceStatus.Text = resource.Status.Value.ToString();
                lblResourceFailure.Text = resource.LastFailure.Description;
                lblFailureTimestamp.Text = resource.LastFailure.Timestamp.ToString("HH:mm:ss.fff");

                switch (resource.Status.Value)
                {
                    case ResourceStatus.Stopping:
                        btnRestartResource.Image = Resources.Start;
                        BackColor = notExecuting;
                        btnRestartResource.FlatAppearance.BorderColor = BackColor;
                        break;

                    case ResourceStatus.Stopped:
                        btnRestartResource.Image = Resources.Start;
                        BackColor = notExecuting;
                        btnRestartResource.FlatAppearance.BorderColor = BackColor;
                        break;

                    case ResourceStatus.Failure:
                        btnRestartResource.Image = Resources.Start;
                        BackColor = failure;
                        btnRestartResource.FlatAppearance.BorderColor = BackColor;
                        break;

                    case ResourceStatus.Starting:
                        btnRestartResource.Image = Resources.Stop;
                        BackColor = notStopped;
                        btnRestartResource.FlatAppearance.BorderColor = BackColor;
                        break;

                    case ResourceStatus.Executing:
                        btnRestartResource.Image = Resources.Stop;
                        BackColor = notStopped;
                        btnRestartResource.FlatAppearance.BorderColor = BackColor;
                        break;
                }

                tip.SetToolTip(lblResourceFailure, lblResourceFailure.Text);
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// Handle the button click event
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="EventArgs"/></param>
        private async void BtnRestartResource_Click(object sender, EventArgs e)
        {
            switch (resource.Status.Value)
            {
                case ResourceStatus.Stopping:
                    await resource.Start();
                    break;

                case ResourceStatus.Stopped:
                    await resource.Start();
                    break;

                case ResourceStatus.Failure:
                    await resource.Start();
                    break;

                case ResourceStatus.Starting:
                    resource.Stop();
                    break;

                case ResourceStatus.Executing:
                    resource.Stop();
                    break;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            if (initialized)
            {
                base.OnResize(e);
                int offset = Size.Width - btnRestartResource.Size.Width - btnRestartResource.Location.X;

                btnRestartResource.Location = new Point(
                    Size.Width - btnRestartResource.Size.Width - 2,
                    btnRestartResource.Location.Y
                );

                lblFailureTimestamp.Location = new Point(
                    btnRestartResource.Location.X - lblFailureTimestamp.Size.Width - 2,
                    lblFailureTimestamp.Location.Y
                );

                lblResourceFailure.Size = new Size(
                    lblResourceFailure.Size.Width + offset - 8,
                    lblResourceFailure.Size.Height
                );
            }
        }

        private void ResourceControl_Cilck(object sender, EventArgs e)
        {
            string message = $"{resource.Code} channels {Environment.NewLine}";

            foreach (IChannel channel in resource.Channels)
                message += $"    - {channel.Code}: {channel}{Environment.NewLine}";

            MessageBox.Show(message, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}