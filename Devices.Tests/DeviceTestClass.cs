using Devices.Template;
using FluentAssertions;
using NUnit.Framework;

namespace Devices.Tests
{
    public class DeviceTestClass
    {
        private NewDevice device;

        [OneTimeSetUp]
        public void Setup()
        {
            device = new NewDevice("DummyDevice");
        }

        [Test]
        public void DoTest()
        {
            device.Channels.Count.Should().NotBe(0);
            device.Parameters.Count.Should().NotBe(0);
        }
    }
}