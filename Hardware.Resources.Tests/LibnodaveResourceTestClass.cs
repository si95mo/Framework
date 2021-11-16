using Core;
using FluentAssertions;
using Hardware.Libnodave;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Hardware.Resources.Tests
{
    [TestFixture]
    public class LibnodaveResourceTestClass
    {
        private LibnodaveResource resource;

        private LibnodaveDigitalInput digitalInput;
        private LibnodaveDigitalOutput digitalOutput;

        private LibnodaveAnalogInput analogInput;
        private LibnodaveAnalogOutput analogOutput;

        [OneTimeSetUp]
        public async Task Setup()
        {
            resource = new LibnodaveResource("LibnodaveResource", "localhost", 80, 10, 0, 2);

            digitalInput = new LibnodaveDigitalInput("DigitalInput", 0, resource);
            digitalOutput = new LibnodaveDigitalOutput("DigitalOutput", 0, resource);

            analogInput = new LibnodaveAnalogInput("AnalogInput", 1, resource, RepresentationBytes.Four, NumericRepresentation.Single);
            analogOutput = new LibnodaveAnalogOutput("AnalogOutput", 1, resource, RepresentationBytes.Four, NumericRepresentation.Int32);

            await resource.Start();
            resource.Status.Value.Should().Be(ResourceStatus.Failure); // No PLC connected
        }

        [Test]
        public async Task Test()
        {
            await Task.Delay(1000);

            // No PLC connected
            digitalInput.Value.Should().Be(default);
            digitalOutput.Value.Should().Be(default);

            analogInput.Value.Should().Be(default);
            analogOutput.Value.Should().Be(default);
        }
    }
}