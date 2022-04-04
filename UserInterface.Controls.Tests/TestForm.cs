using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UserInterface.Forms;

namespace UserInterface.Controls.Tests
{
    public partial class TestForm : CustomForm
    {
        private int counter = 0;
        private Color onColor = Color.Green;
        private Color offColor = Color.Red;

        public TestForm()
        {
            InitializeComponent();

            ledControl.Blink(1000);

            Timer ledTimer = new Timer
            {
                Interval = 10000
            };
            ledTimer.Tick += LedTimer_Tick;

            ledTimer.Start();

            Timer progressTimer = new Timer
            {
                Interval = 1000
            };
            progressTimer.Tick += ProgressTimer_Tick;

            progressTimer.Start();
        }

        private void ProgressTimer_Tick(object sender, EventArgs e)
        {
            progressBar.Invoke(new MethodInvoker(() => progressBar.Value += 1));
        }

        private void LedTimer_Tick(object sender, EventArgs e)
        {
            ledControl.Invoke(new MethodInvoker(() =>
                    {
                        if (++counter % 2 == 0)
                            ledControl.Color = onColor;
                        else
                            ledControl.Color = offColor;
                    }
                )
            );
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            dgvControl.DataSource = new List<List<string>>
            {
                new List<string>{ "AAA", "BBB", "CCC" },
                new List<string>{ "111", "222", "333", "444", "555" }
            };

            ResourceTestForm form = new ResourceTestForm();
            form.Show();

            SpectrumPlotterForm plotterForm = new SpectrumPlotterForm();
            plotterForm.Show();

            ChartControlForm chartControlForm = new ChartControlForm();
            chartControlForm.Show();
        }

        private void BtnShowCustomMessageBox_Click(object sender, EventArgs e)
        {
            ShowAlert(title: "Debug test", message: $"Clicked on: {DateTime.Now}");
        }
    }
}