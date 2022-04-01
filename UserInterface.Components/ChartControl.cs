using Core.Parameters;
using Hardware;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    /// <summary>
    /// Implement a chart <see cref="UserControl"/> that display the values
    /// of a <see cref="NumericParameter"/> or a <see cref="WaveformParameter"/>
    /// </summary>
    public partial class ChartControl : UserControl
    {
        private const int BufferSize = 128;

        private NumericParameter numericParameter;
        private WaveformParameter waveformParameter;

        private LineSeries series;

        private Task updateTask;
        private bool stopUpdateTask;

        /// <summary>
        /// Create a new instance of <see cref="ChartControl"/>
        /// </summary>
        public ChartControl()
        {
            InitializeComponent();

            chart.DataTooltip.IsEnabled = false;
            chart.DisableAnimations = true;
            chart.Hoverable = false;

            updateTask = new Task(async () =>
                {
                    stopUpdateTask = false;
                    int i = 0;

                    while (!stopUpdateTask)
                    {
                        UpdateChartSeries();
                        await Task.Delay(250);
                    }
                }
            );
        }

        /// <summary>
        /// Update the chart series for a <see cref="NumericParameter"/>
        /// </summary>
        private void UpdateChartSeries()
        {
            if (series.Values.Count > 0 && series.Values.Count % BufferSize == 0)
                series.Values.RemoveAt(0);

            series.Values.Add(numericParameter.Value);
        }

        /// <summary>
        /// Update the chart series for a <see cref="WaveformParameter"/>
        /// </summary>
        /// <param name="value">The value</param>
        private void UpdateChartSeries(double[] value)
        {
            series.Values.Clear();
            value.ToList().ForEach(x => series.Values.Add(x));
        }

        public void SetParameter(NumericParameter parameter)
        {
            InitChart(parameter.Code);

            numericParameter = new NumericParameter(
                $"{parameter.Code}.ChartNumericParameter",
                value: parameter.Value,
                measureUnit: parameter.MeasureUnit,
                format: parameter.Format
            );
            parameter.ConnectTo(numericParameter);

            if (waveformParameter != null)
                waveformParameter.ValueChanged -= WaveformParameter_ValueChanged;

            updateTask.Start();
            numericParameter.ValueChanged += NumericParameter_ValueChanged;
        }

        public void SetParameter(WaveformParameter parameter)
        {
            InitChart(parameter.Code);

            waveformParameter = new WaveformParameter(
                $"{parameter.Code}.ChartWaveformParameter",
                value: parameter.Value,
                measureUnit: parameter.MeasureUnit,
                format: parameter.Format
            );
            parameter.ConnectTo(waveformParameter);

            if (numericParameter != null)
                numericParameter.ValueChanged -= NumericParameter_ValueChanged;

            stopUpdateTask = true;
            waveformParameter.ValueChanged += WaveformParameter_ValueChanged;
        }

        /// <summary>
        /// Initialize the chart series
        /// </summary>
        /// <param name="seriesName">The series name</param>
        private void InitChart(string seriesName)
        {
            series = new LineSeries()
            {
                Title = seriesName,
                Values = new ChartValues<double>(),
                Fill = System.Windows.Media.Brushes.Transparent
            };

            series.PointGeometry = null;

            chart.Series.Clear();
            chart.Series.Add(series);
        }

        private void NumericParameter_ValueChanged(object sender, Core.ValueChangedEventArgs e)
            => UpdateChartSeries();

        private void WaveformParameter_ValueChanged(object sender, Core.ValueChangedEventArgs e)
            => UpdateChartSeries(waveformParameter.Value);
    }
}
