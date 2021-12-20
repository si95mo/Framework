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

        [OneTimeSetUp]
        public async Task Setup()
        {
            resource = new Snap7Resource("Snap7Resource", ipAddress, 0, 0, 32);

            await resource.Start();
            resource.Status.Value.Should().Be(ResourceStatus.Executing);
        }

        [Test]
        public void Test()
        {
            Console.WriteLine(resource);
        }
    }
}