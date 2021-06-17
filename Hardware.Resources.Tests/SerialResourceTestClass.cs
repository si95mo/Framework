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
            resource = new SerialResource(nameof(resource), "COM99");
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
            resource.Send(message);

            resource.Status.Should().Be(ResourceStatus.Executing);
        }
    }
}
