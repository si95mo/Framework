using Core.Converters;
using Core.DataStructures;
using Hardware;
using Hardware.WaveformGenerator;
using System;
using System.Windows.Forms;

namespace UserInterface.Dashboards.Tests
{
    public partial class TestForm : Form
    {
        private WaveformGeneratorResource resource;
        private AnalogOutput output;
        private AnalogInput input;

        public TestForm()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            output = new AnalogOutput($"AnalogOutput", "V", "0.000");
            input = new AnalogInput($"AnalogInput", "V", "0.000");

            resource = new WaveformGeneratorResource(
                $"WaveformGenerator",
                WaveformType.Random,
                0d,
                0d,
                output,
                0d,
                0d,
                250
            );
            resource.Start();

            output.ConnectTo(input, new GenericConverter<double, double>(new Func<double, double>((x) => 10 + x)));

            ServiceBroker.Initialize();
            ServiceBroker.Add<IChannel>(output);
            ServiceBroker.Add<IChannel>(input);
        }
    }
}