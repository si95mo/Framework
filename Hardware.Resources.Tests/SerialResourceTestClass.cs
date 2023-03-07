using Diagnostic;
using FluentAssertions;
using NUnit.Framework;
using System.Text;
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

            resource = new SerialResource(nameof(resource), "COM3", Encoding.ASCII);
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
                resource.Output.EncodedValue = message;
            }
            else
                resource.LastFailure.Description.Should().NotBe(string.Empty);
        }
    }
}