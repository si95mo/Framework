using Hardware;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.Controls.Tests
{
    public partial class SpectrumPlotterForm : Form
    {
        private LineSeries series;
        private MultiSampleAnalogInput input;

        public SpectrumPlotterForm()
        {
            InitializeComponent();

            series = new LineSeries();
            series.Title = "Spectrum";
            series.Values = new ChartValues<double>();

            chart.Series.Clear();
            chart.Series.Add(series);

            input = new MultiSampleAnalogInput("Waveform");
            input.ValueChanged += Input_ValueChanged;

            bgWorker.RunWorkerAsync();
        }

        private async void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            double[] samples = new double[1000];

            while(true)
            {
                for (int i = 0; i < 1000; i++)
                {
                    samples[i] = Math.Sin(i);
                    await Task.Delay(1);
                }

                    input.Value = samples;
            }
        }

        private void Input_ValueChanged(object sender, Core.ValueChangedEventArgs e)
        {
            series.Values.Clear();

            for (int i = 0; i < input.Value.Length; i++)
                series.Values.Add(input.Value[i]);
        }
    }
}
