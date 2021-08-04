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

            resource = new SerialResource(nameof(resource), "COM99");
            resource.Start();

            if (resource.LastFailure == resource.LastFailure.Default)
            {
                resource.IsOpen.Should().BeTrue();
                resource.Status.Should().Be(ResourceStatus.Executing);
            }
            else
            {
                resource.LastFailure.Description.Should().NotBe("");
                resource.Status.Should().Be(ResourceStatus.Failure);
            }
        }

        [OneTimeTearDown]
        public void Dispose()
        {
            if (resource.LastFailure == resource.LastFailure.Default)
            {
                resource.Stop();

                resource.IsOpen.Should().BeFalse();
                resource.Status.Should().Be(ResourceStatus.Stopped);
            }
            else
                resource.LastFailure.Description.Should().NotBe("");
        }

        [Test]
        [TestCase("Hello")]
        [TestCase("Hello world!")]
        public void SendMessage(string message)
        {
            if (resource.LastFailure == resource.LastFailure.Default)
            {
                resource.Send(message);

                resource.Status.Should().Be(ResourceStatus.Executing);
            }
            else
                resource.LastFailure.Description.Should().NotBe("");
        }
    }
}