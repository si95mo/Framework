using Core.Parameters;
using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    /// <summary>
    /// Implement a chart <see cref="UserControl"/> that display the values of a <see cref="NumericParameter"/> or a <see cref="WaveformParameter"/>.
    /// If the <see cref="ChartControl"/> should display static data (i.e. no <see cref="NumericParameter"/> or <see cref="WaveformParameter"/>), then use
    /// <see cref="Series"/> to manipulate the <see cref="ChartControl"/>
    /// </summary>
    public partial class ChartControl : UserControl
    {
        private const int BufferSize = 128;

        private NumericParameter numericParameter;
        private WaveformParameter waveformParameter;

        private Series series;

        private Task updateTask;
        private bool stopUpdateTask;

        /// <summary>
        /// The <see cref="LineSeries"/>
        /// </summary>
        public Series Series 
        { 
            get => series;
            set
            {
                series = value;
                chart.Series.Clear();
                chart.Series.Add(series);
            }
        }

        /// <summary>
        /// Create a new instance of <see cref="ChartControl"/>
        /// </summary>
        public ChartControl()
        {
            InitializeComponent();

            chart.DataTooltip = null;
            chart.Hoverable = false;
            chart.DisableAnimations = true;

            updateTask = new Task(async () =>
                {
                    stopUpdateTask = false;

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
            series.Values = value.AsChartValues();
        }

        /// <summary>
        /// Set the <see cref="ChartControl"/> associated <see cref="NumericParameter"/>
        /// </summary>
        /// <param name="parameter">The <see cref="NumericParameter"/> to connect</param>
        public void SetParameter(NumericParameter parameter)
        {
            InitializeChart(parameter.Code);

            numericParameter = new NumericParameter($"{parameter.Code}.ChartNumericParameter", parameter.Value, parameter.MeasureUnit, parameter.Format);
            parameter.ConnectTo(numericParameter);

            if (waveformParameter != null)
                waveformParameter.ValueChanged -= WaveformParameter_ValueChanged;

            updateTask.Start();
            numericParameter.ValueChanged += NumericParameter_ValueChanged;
        }

        /// <summary>
        /// Set the <see cref="ChartControl"/> associated <see cref="WaveformParameter"/>
        /// </summary>
        /// <param name="parameter">The <see cref="WaveformParameter"/> to connect</param>
        public void SetParameter(WaveformParameter parameter)
        {
            InitializeChart(parameter.Code);

            waveformParameter = new WaveformParameter($"{parameter.Code}.ChartWaveformParameter", parameter.Value, parameter.MeasureUnit, parameter.Format);
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
        private void InitializeChart(string seriesName)
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