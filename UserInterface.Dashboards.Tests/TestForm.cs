using Hardware;
using Hardware.WaveformGenerator;
using System.Windows.Forms;

namespace UserInterface.Dashboards.Tests
{
    public partial class TestForm : Form
    {
        private WaveformGeneratorResource resource;
        private AnalogOutput output;

        public TestForm()
        {
            InitializeComponent();

            output = new AnalogOutput($"AnalogOutput", "V", "0.000");
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

            dashboard.Dashboard.ControlAdded += Dashboard_ControlAdded;
        }

        private void Dashboard_ControlAdded(object sender, ControlEventArgs e)
        {
            if (e.Control is AnalogReadControl)
                (e.Control as AnalogReadControl).SetChannel(output);
        }
    }
}