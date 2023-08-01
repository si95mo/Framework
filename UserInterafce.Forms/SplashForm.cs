using System;
using System.Collections.Generic;
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

        public SplashForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        public SplashForm(Dictionary<string, Func<Task>> actions, Form formToLaunch, Task disposingTask = null) : this()
        {
            Actions = actions;
            FormToLaunch = formToLaunch;

            DisposingTask = disposingTask;
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
            }
            else
                BeginInvoke(new Action(async () => await InitializeAsync()));
        }
    }
}
