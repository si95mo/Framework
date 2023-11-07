using FluentAssertions;
using Hardware.Twincat;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Hardware.Resources.Tests
{
    [TestFixture]
    public class TwincatResourceTestClass
    {
        // Check which is correct for the local ads server. The amsnet address is not used in case of a local ads server
        private readonly string amsNetAddress = "169.254.174.61.1.1";

        private readonly int port = 851; // TwinCAT - local ads server

        private TwincatResource resource;

        private TwincatAnalogInput analogIn;
        private TwincatDigitalInput digitalIn;
        private TwincatAnalogOutput analogOut;
        private TwincatDigitalOutput digitalOut;

        [OneTimeSetUp]
        public async Task Setup()
        {
            resource = new TwincatResource("TwincatResource", port, 10, 10);

            await resource.Start();
            resource.Status.Value.Should().BeOneOf(ResourceStatus.Executing, ResourceStatus.Failure);

            analogIn = new TwincatAnalogInput("AnalogInputVariableName", "GVL.KF10_1", resource)
            {
                Description = "KF10",
                Symbolic = "I10.0"
            };
            digitalIn = new TwincatDigitalInput("DigitalInputVariableName", "GVL.KF11_1", resource)
            {
                Description = "KF11",
                Symbolic = "I11.0"
            };
            TwincatAnalogInput analogInDummy = new TwincatAnalogInput("AnalogInputVariableNameDummy", "GVL.KF10_2", resource)
            {
                Description = "KF10",
                Symbolic = "I10.1"
            };
            analogOut = new TwincatAnalogOutput("AnalogOutputVariableName", "GVL.KF12_1", resource)
            {
                Description = "KF12",
                Symbolic = "O10.0"
            };
            digitalOut = new TwincatDigitalOutput("DigitalOutputVariableName", "GVL.KF13_1", resource)
            {
                Description = "KF13",
                Symbolic = "O11.0"
            };
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            resource.Stop();
        }

        [Test]
        public async Task Test()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test_results", "test_gvl.txt");
            File.Delete(path);

            resource.ExportChannelsToGvl(path);
            File.Exists(path).Should().BeTrue();

            for (int i = 1; i <= 10; i++)
            {
                analogOut.Value = i;
                digitalOut.Value = !digitalOut.Value;

                await Task.Delay(10);

                Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} >> {analogIn.VariableName}: {analogIn.Value}, {digitalIn.VariableName}: {digitalIn.Value}");
            }
        }
    }
}