using Core;
using FluentAssertions;
using Hardware.Libnodave;
using NUnit.Framework;
using System;
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
        [Timeout(10000)]
        public async Task Setup()
        {
            resource = new LibnodaveResource("LibnodaveResource", "192.168.0.1", 102, 16, 0, 1, 1000);

            await resource.Start();
            resource.Status.Value.Should().Be(ResourceStatus.Executing); // No PLC connected

            analogInput = new LibnodaveAnalogInput("AnalogInput", 1, resource, RepresentationBytes.One, NumericRepresentation.UInt16);
        }

        [Test]
        public async Task Test()
        {
            await Task.Delay(1000);
            Console.WriteLine(analogInput.ToString());
        }
    }
}