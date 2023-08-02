using Diagnostic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.Forms
{
    public partial class SplashForm : Form
    {
        private const string BaseStateMessage = "State:";

        /// <summary>
        /// The collection of action to perform in the <see cref="SplashForm"/>, by name
        /// </summary>
        public Dictionary<string, Func<Task>> Actions { get; private set; }

        /// <summary>
        /// The <see cref="Form"/> to launch after initialization completed
        /// </summary>
        public Form FormToLaunch { get; private set; }

        public Task DisposingTask { get; private set; }

        protected SplashForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// Create a new instance of <see cref="SplashForm"/>
        /// </summary>
        /// <param name="actions">The collection of actions to do in order to initialize the application</param>
        /// <param name="formToLaunch">The <see cref="Form"/> to launch after the initialization</param>
        /// <param name="customerLogoImagePath">The customer logo image path</param>
        /// <param name="projectCode">The project code (i.e. the text shown on the <paramref name="formToLaunch"/>)</param>
        /// <param name="disposingTask">The (eventual) disposing <see cref="Task"/> to call on shut down</param>
        public SplashForm(Dictionary<string, Func<Task>> actions, Form formToLaunch, string customerLogoImagePath, string projectCode, Task disposingTask = null) 
            : this()
        {
            Actions = actions;
            FormToLaunch = formToLaunch;
            DisposingTask = disposingTask;

            if (File.Exists(customerLogoImagePath))
            {
                pbxClient.Image = new Bitmap(customerLogoImagePath);
                pbxClient.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
                Logger.Error($"Unable to load customer logo on {nameof(SplashForm)}. File specified at \"{customerLogoImagePath}\" does not exist");

            FormToLaunch.Text = projectCode;
        }

        private async void SplashForm_Load(object sender, EventArgs e)
        {
            await InitializeAsync();

            Hide();

            FormToLaunch.AutoScaleMode = AutoScaleMode.Inherit;
            FormToLaunch.StartPosition = FormStartPosition.CenterScreen;
            FormToLaunch.Closed += async (s, args) =>
            {
                if (DisposingTask != null)
                    await DisposingTask;

                Close();
            };
            FormToLaunch.Show();
        }

        private async Task InitializeAsync()
        {
            if (!InvokeRequired)
            {
                int increment = 100 / Actions.Count;
                int counter = 0;

                Stopwatch timer = Stopwatch.StartNew();
                await Logger.DebugAsync($"Initialization starting");

                // Step 0
                lblStatus.Text = $"{BaseStateMessage} application initialization... ({++counter} / {Actions.Count})";
                prbProgress.Value = 0;
                await Task.Delay(TimeSpan.FromMilliseconds(500d));

                foreach(KeyValuePair<string, Func<Task>> action in Actions)
                {
                    lblStatus.Text = $"{BaseStateMessage} {action.Key} ({++counter} / {Actions.Count})";
                    prbProgress.Value += increment;
                    await action.Value();
                }

                // Last step
                lblStatus.Text = $"{BaseStateMessage} initialization completed... ({++counter} / {Actions.Count})";
                prbProgress.Value = 100;

                timer.Stop();
                await Logger.InfoAsync($"Initialization done. It took a total {timer.Elapsed.TotalMilliseconds:0.0} [ms] to complete");
            }
            else
                BeginInvoke(new Action(async () => await InitializeAsync()));
        }
    }
}
