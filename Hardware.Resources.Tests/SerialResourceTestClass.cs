using Diagnostic;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Hardware.Resources.Tests
{
    public class SerialResourceTestClass
    {
        private SerialResource resource;

        [OneTimeSetUp]
        public async Task Setup()
        {
            Logger.Initialize();

            resource = new SerialResource(nameof(resource), "COM3");
            await resource.Start();

            resource.IsOpen.Should().BeTrue();
            resource.Status.Value.Should().Be(ResourceStatus.Executing);
        }

        [OneTimeTearDown]
        public void Dispose()
        {
            resource.Stop();

            resource.IsOpen.Should().BeFalse();
            resource.Status.Value.Should().Be(ResourceStatus.Stopped);
        }

        [Test]
        [TestCase("Hello")]
        [TestCase("Hello world!")]
        public void SendMessage(string message)
        {
            if (resource.Status.Value == ResourceStatus.Executing)
            {
                resource.Send(message);
            }
            else
                resource.LastFailure.Description.Should().NotBe("");
        }
    }
}