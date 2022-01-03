using Hardware;
using LiveCharts;
using LiveCharts.Wpf;
using Mathematics;
using Signal.Processing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

            series = new LineSeries()
            {
                Title = "Spectrum",
                Values = new ChartValues<double>()
            };

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
                    await Task.Delay(0);
                }

                input.Value = samples;
            }
        }

        private void Input_ValueChanged(object sender, Core.ValueChangedEventArgs e)
        {
            double[] magnitudes = Mathematics.Mathematics.CalculateMagnitudes(Fourier.FFT(input.Value));

            series.Values.Clear();
            for (int i = 0; i < magnitudes.Length; i++)
                series.Values.Add(magnitudes[i]);
        }
    }
}
