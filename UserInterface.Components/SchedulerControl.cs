using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
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

            chart.DataTooltip = null;
            chart.Hoverable = false;
            chart.DisableAnimations = true;

            series = new LineSeries
            {
                Title = "Load",
                Values = new ChartValues<double>(),
                PointGeometry = null
            };
            
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
