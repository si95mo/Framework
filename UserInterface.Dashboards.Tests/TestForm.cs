using Core.Converters;
using Core.DataStructures;
using Hardware;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.Dashboards.Tests
{
    public partial class TestForm : Form
    {
        private AnalogOutput analogOutput;
        private AnalogInput analogInput;
        private DigitalInput digitalInput;
        private DigitalOutput digitalOutput;
        private MultiSampleAnalogInput multiSampleAnalogInput;

        public TestForm()
        {
            InitializeComponent();

            MethodWrapper.Wrap(this);

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            analogOutput = new AnalogOutput("AnalogOutput", "V", "0.000");
            analogInput = new AnalogInput("AnalogInput", "V", "0.000");
            digitalOutput = new DigitalOutput("DigitalOutput");
            digitalInput = new DigitalInput("DigitalInput");
            multiSampleAnalogInput = new MultiSampleAnalogInput("MultiSampleAnalogInput", "V", "0.000");

            analogOutput.ConnectTo(
                analogInput,
                new GenericConverter<double, double>(new Func<double, double>((x) => 10 + x))
            );
            digitalOutput.ConnectTo(
                digitalInput,
                new GenericConverter<bool, bool>(new Func<bool, bool>((x) => !x))
            );

            Task t = new Task(async () =>
                {
                    Random rnd = new Random(Guid.NewGuid().GetHashCode());
                    double[] values = new double[100];

                    while (true)
                    {
                        for (int i = 0; i < 100; i++)
                            values[i] = rnd.NextDouble();

                        multiSampleAnalogInput.Value = values;

                        await Task.Delay(100);
                    }
                }
            );
            t.Start();

            ServiceBroker.Initialize();
            ServiceBroker.Add<IChannel>(analogOutput);
            ServiceBroker.Add<IChannel>(analogInput);
            ServiceBroker.Add<IChannel>(digitalOutput);
            ServiceBroker.Add<IChannel>(digitalInput);
            ServiceBroker.Add<IChannel>(multiSampleAnalogInput);
        }
    }
}