using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Converters;
using Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace UserInterface.Controls
{
    public partial class SchedulerControl : UserControl
    {
        public const int BufferSize = 128;

        private readonly IScheduler scheduler;
        private Series series;

        public SchedulerControl(IScheduler scheduler)
        {
            InitializeComponent();

            lblCode.Text = scheduler.Code;

            chart.DataTooltip = null;
            chart.Hoverable = false;
            chart.DisableAnimations = true;

            series = new LineSeries
            {
                Title = "Load",
                Values = new ChartValues<double>(),
                PointGeometry = null,
                Stroke = Brushes.Red,
                Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0))
            };
            series.Fill.Opacity = 0.1;

            chart.AxisY.Clear();
            chart.AxisY.Add(new Axis());
            chart.AxisY[0].MinValue = -4d;
            chart.AxisY[0].MaxValue = 104d;

            chart.Series.Clear();
            chart.Series.Add(series);
            chart.InvalidateVisual();

            this.scheduler = scheduler;

            Task t = new Task(async () =>
                {
                    while (true)
                    {
                        if (!InvokeRequired)
                            UpdateChart(scheduler.Load.Value);
                        else
                            BeginInvoke(new Action(() => UpdateChart(scheduler.Load.Value)));

                        await Task.Delay(250);
                    }
                }
            );
            t.Start();
        }

        private void UpdateChart(double load)
        {
            if (series.Values.Count > 0 && series.Values.Count == BufferSize)
                series.Values.RemoveAt(0);

            series.Values.Add(load);
        }
    }
}
