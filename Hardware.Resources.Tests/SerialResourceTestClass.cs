using Diagnostic;
using FluentAssertions;
using NUnit.Framework;

namespace Hardware.Resources.Tests
{
    public class SerialResourceTestClass
    {
        private SerialResource resource;

        [OneTimeSetUp]
        public void Init()
        {
            Logger.Initialize();

            resource = new SerialResource(nameof(resource), "COM3");
            resource.Start();

            resource.IsOpen.Should().BeTrue();
            resource.Status.Should().Be(ResourceStatus.Executing);
        }

        [OneTimeTearDown]
        public void Dispose()
        {
            resource.Stop();

            resource.IsOpen.Should().BeFalse();
            resource.Status.Should().Be(ResourceStatus.Stopped);
        }

        [Test]
        [TestCase("Hello")]
        [TestCase("Hello world!")]
        public void SendMessage(string message)
        {
            if (resource.Status == ResourceStatus.Executing)
            {
                resource.Send(message);
            }
            else
                resource.LastFailure.Description.Should().NotBe("");
        }
    }
}