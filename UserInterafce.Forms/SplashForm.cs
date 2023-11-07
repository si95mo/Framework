using Diagnostic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.Forms
{
    /// <summary>
    /// Define a splash <see cref="Form"/>
    /// </summary>
    public partial class SplashForm : Form
    {
        private const string BaseStateMessage = "State:";

        /// <summary>
        /// The collection of action to perform in the <see cref="SplashForm"/> at startup, by name
        /// </summary>
        public Dictionary<string, Func<Task>> StartupActions { get; private set; }

        /// <summary>
        /// The collection of action to perform in the <see cref="SplashForm"/> at close, by name
        /// </summary>
        public Dictionary<string, Func<Task>> DisposeActions { get; private set; }

        /// <summary>
        /// The <see cref="Form"/> to launch after initialization completed
        /// </summary>
        public Form FormToLaunch { get; private set; }

        private Type formToLaunchType;
        private string projectCode;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );

        /// <summary>
        /// Create a new instance of <see cref="SplashForm"/>
        /// </summary>
        protected SplashForm()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;
            Opacity = 0.9;
        }

        /// <summary>
        /// Create a new instance of <see cref="SplashForm"/>
        /// </summary>
        /// <param name="startupActions">The collection of actions to do in order to initialize the application</param>
        /// <param name="formToLaunchType">The actual <see cref="Type"/> of the <see cref="Form"/> to launch after the initialization operations</param>
        /// <param name="customerLogoImagePath">The customer logo image path</param>
        /// <param name="projectCode">The project code (i.e. the text shown on the <paramref name="formToLaunch"/>)</param>
        /// <param name="disposeActions">The (eventual) disposing <see cref="Task"/> to call on shut down</param>
        public SplashForm(Dictionary<string, Func<Task>> startupActions, Type formToLaunchType, string customerLogoImagePath, string projectCode,
            Dictionary<string, Func<Task>> disposeActions = null) : this()
        {
            StartupActions = startupActions;
            DisposeActions = disposeActions;

            this.formToLaunchType = formToLaunchType;

            if (File.Exists(customerLogoImagePath))
            {
                pbxClient.Image = new Bitmap(customerLogoImagePath);

                Size imageSize = pbxClient.Image.Size;
                Size fitSize = pbxClient.ClientSize;
                pbxClient.SizeMode = imageSize.Width > fitSize.Width || imageSize.Height > fitSize.Height ?
                    PictureBoxSizeMode.Zoom :
                    PictureBoxSizeMode.CenterImage;
            }
            else
                Logger.Error($"Unable to load customer logo on {nameof(SplashForm)}. File specified at \"{customerLogoImagePath}\" does not exist");

            this.projectCode = projectCode;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Round corners
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            base.OnPaint(e);
        }

        private async void SplashForm_Load(object sender, EventArgs e)
        {
            // Initialize everything first
            await InitializeAsync();

            // Then hide the splash form
            Hide();

            // Create the form to launch instance
            FormToLaunch = (Form)Activator.CreateInstance(formToLaunchType);

            // Apply some value to its properties
            FormToLaunch.AutoScaleMode = AutoScaleMode.Inherit;
            FormToLaunch.StartPosition = FormStartPosition.CenterScreen;
            FormToLaunch.Text = projectCode;

            // And finally connect to its event handlers
            FormToLaunch.Closed += FormToLaunch_Closed;
            FormToLaunch.Show();
        }

        private async Task InitializeAsync()
        {
            if (!InvokeRequired)
            {
                int increment = 100 / StartupActions.Count;
                int counter = 0;

                Stopwatch timer = Stopwatch.StartNew();
                await Logger.DebugAsync($"Initialization starting");

                // Step 0
                lblStatus.Text = $"{BaseStateMessage} application initialization... ({++counter} / {StartupActions.Count})";
                prbProgress.Value = 0;
                await Task.Delay(TimeSpan.FromMilliseconds(500d));

                foreach (KeyValuePair<string, Func<Task>> action in StartupActions)
                {
                    lblStatus.Text = $"{BaseStateMessage} {action.Key} ({++counter} / {StartupActions.Count})";
                    prbProgress.Value += increment;

                    await action.Value();
                }

                // Last step
                lblStatus.Text = $"{BaseStateMessage} initialization completed... ({++counter} / {StartupActions.Count})";
                prbProgress.Value = 100;

                timer.Stop();
                await Logger.InfoAsync($"Initialization done. It took a total {timer.Elapsed.TotalMilliseconds:0.0} [ms] to complete");
            }
            else
                BeginInvoke(new Action(async () => await InitializeAsync()));
        }

        private async void FormToLaunch_Closed(object sender, EventArgs e)
        {
            Show(); // Previously hidden
            await Task.Delay(TimeSpan.FromMilliseconds(1000d));

            // Step 0
            lblStatus.Text = $"{BaseStateMessage} application shutting down... ({1} / {(DisposeActions?.Count > 0 ? DisposeActions.Count : 1)})";
            prbProgress.Value = 0;
            await Task.Delay(TimeSpan.FromMilliseconds(1000d));

            if (DisposeActions != null)
            {
                if (!InvokeRequired)
                    await DisposeAsync();
                else
                    BeginInvoke(new Action(() => FormToLaunch_Closed(sender, e)));
            }

            Close();
        }

        private async Task DisposeAsync()
        {
            int increment = 100 / DisposeActions.Count;
            int counter = 1; // First step already done

            Stopwatch timer = Stopwatch.StartNew();
            await Logger.DebugAsync($"Shut down starting");

            foreach (KeyValuePair<string, Func<Task>> action in DisposeActions)
            {
                lblStatus.Text = $"{BaseStateMessage} {action.Key} ({++counter} / {DisposeActions.Count})";
                prbProgress.Value += increment;

                await action.Value();
                await Task.Delay(TimeSpan.FromMilliseconds(500d));
            }

            timer.Stop();
            await Logger.InfoAsync($"Shut down done. It took a total {timer.Elapsed.TotalMilliseconds:0.0} [ms] to complete");
        }
    }
}