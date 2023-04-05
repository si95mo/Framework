using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using Tasks;

namespace UserInterface.Controls
{
    public partial class SchedulerControl : UserControl
    {
        public const int BufferSize = 128;
        public const double Opacity = 0.1;

        private readonly Color green = Color.FromRgb(0xBD, 0xDC, 0x04);
        private readonly Color yellow = Color.FromRgb(0xFE, 0xD0, 0x00);
        private readonly Color red = Color.FromRgb(0xD2, 0x04, 0x2D);

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
                Stroke = new SolidColorBrush(green),
                Fill = new SolidColorBrush(green),
                LineSmoothness = 0.1
            };
            series.Fill.Opacity = Opacity;

            chart.AxisY.Clear();
            chart.AxisY.Add(new Axis());
            chart.AxisY[0].MinValue = -4d;
            chart.AxisY[0].MaxValue = 104d;

            chart.Series.Clear();
            chart.Series.Add(series);
            chart.InvalidateVisual();

            this.scheduler = scheduler;
            UpdateChartColor(scheduler.Load.Value);

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

            scheduler.Load.ValueChanged += Load_ValueChanged;
        }

        private void UpdateChart(double load)
        {
            if (series.Values.Count > 0 && series.Values.Count == BufferSize)
                series.Values.RemoveAt(0);

            series.Values.Add(load);
        }

        private void UpdateChartColor(double load)
        {
            LineSeries series = chart.Series[0] as LineSeries;

            if (load <= 33.333)
            {
                series.Stroke = new SolidColorBrush(green);
                series.Fill = new SolidColorBrush(green);
            }
            else if (load <= 66.666)
            {
                series.Stroke = new SolidColorBrush(yellow);
                series.Fill = new SolidColorBrush(yellow);
            }
            else // load > 66.666
            {
                series.Stroke = new SolidColorBrush(red);
                series.Fill = new SolidColorBrush(red);
            }

            series.Fill.Opacity = Opacity;
        }

        private void Load_ValueChanged(object sender, Core.ValueChangedEventArgs e)
        {
            if (!InvokeRequired)
                UpdateChartColor(e.NewValueAsDouble);
            else
                BeginInvoke(new Action(() => Load_ValueChanged(sender, e)));
        }
    }
}
