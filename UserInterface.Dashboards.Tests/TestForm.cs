using Core.Converters;
using Core.DataStructures;
using Hardware;
using Hardware.WaveformGenerator;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.Dashboards.Tests
{
    public partial class TestForm : Form
    {
        private WaveformGeneratorResource resource;

        private AnalogOutput analogOutput;
        private AnalogInput analogInput;
        private DigitalInput digitalInput;
        private DigitalOutput digitalOutput;

        public TestForm()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            analogOutput = new AnalogOutput($"AnalogOutput", "V", "0.000");
            analogInput = new AnalogInput($"AnalogInput", "V", "0.000");
            digitalOutput = new DigitalOutput($"DigitalOutput");
            digitalInput = new DigitalInput($"DigitalInput");

            resource = new WaveformGeneratorResource(
                $"WaveformGenerator",
                WaveformType.Random,
                0d,
                0d,
                analogOutput,
                0d,
                0d,
                250
            );
            resource.Start();

            analogOutput.ConnectTo(
                analogInput, 
                new GenericConverter<double, double>(new Func<double, double>((x) => 10 + x))
            );
            digitalOutput.ConnectTo(digitalInput);

            ServiceBroker.Initialize();
            ServiceBroker.Add<IChannel>(analogOutput);
            ServiceBroker.Add<IChannel>(analogInput);
            ServiceBroker.Add<IChannel>(digitalOutput);
            ServiceBroker.Add<IChannel>(digitalInput);

            Task digitalOutputTask = new Task(async () =>
                {
                    while (true)
                    {
                        digitalOutput.Value = !digitalOutput.Value;
                        await Task.Delay(1000);
                    }
                }
            );
            digitalOutputTask.Start();
        }
    }
}