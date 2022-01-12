using Core;
using FluentAssertions;
using Hardware.Snap7;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Hardware.Resources.Tests
{
    [TestFixture]
    public class Snap7ResourceTestClass
    {
        private Snap7Resource resource;
        private string ipAddress = "0.0.0.0";

        private Snap7AnalogInput analogInput;

        [OneTimeSetUp]
        public async Task Setup()
        {
            resource = new Snap7Resource("Snap7Resource", ipAddress, rack: 0, slot: 0, pollingInterval: 1000);
            resource.AddDataBlock(1, 16);

            await resource.Start();
            // resource.Status.Value.Should().Be(ResourceStatus.Executing);

            analogInput = new Snap7AnalogInput("Snap7AnalogInput", memoryAddress: 0, dataBlock: 1, resource, RepresentationBytes.Two, NumericRepresentation.Int16);
        }

        [Test]
        public void Test()
        {
            analogInput.Value.Should().Be(default);
        }
    }
}