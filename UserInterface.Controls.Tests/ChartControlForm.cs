using Core.Parameters;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.Controls.Tests
{
    public partial class ChartControlForm : Form
    {
        private NumericParameter numericParameter;
        private WaveformParameter waveformParameter;

        public ChartControlForm()
        {
            InitializeComponent();

            numericParameter = new NumericParameter(Guid.NewGuid().ToString());
            waveformParameter = new WaveformParameter(Guid.NewGuid().ToString());

            chrNumeric.SetParameter(numericParameter);
            chrWaveform.SetParameter(waveformParameter);

            bgWorker.RunWorkerAsync();
        }
        private async void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            double[] samples = new double[1000];
            Random random = new Random(Guid.NewGuid().GetHashCode());
            bool useSine = true;

            while (true)
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (useSine)
                        samples[i] = Math.Sin(i) + random.NextDouble();
                    else
                        samples[i] = Math.Cos(i) + random.NextDouble();

                    useSine = !useSine;
                }

                waveformParameter.Value = samples;
                numericParameter.Value = random.Next(-32, 32);

                await Task.Delay(1200);
            }
        }
    }
}
