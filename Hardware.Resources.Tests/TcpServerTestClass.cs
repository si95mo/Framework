using Core.Conditions;
using Extensions;
using FluentAssertions;
using Hardware.Tcp;
using NUnit.Framework;
using System.Text;
using System.Threading.Tasks;

namespace Hardware.Resources.Tests
{
    [TestFixture]
    public class TcpServerTestClass
    {
        private TcpServer server;
        private DummyCondition valueReceivedFromServer;

        [OneTimeSetUp]
        [Timeout(10000)]
        public void Setup()
        {
            server = new TcpServer("TcpServer", "localhost", 1234, Encoding.ASCII);
            server.Start();

            server.Status.Value.Should().Be(ResourceStatus.Executing);

            valueReceivedFromServer = new DummyCondition("ValueReceived");
        }

        [TearDown]
        public void TearDown()
        {
            server.Stop();
        }

        [Test]
        public async Task Test()
        {
            server.StreamInput.ValueChanged += StreamInput_ValueChanged;

            await server.WaitFor(valueReceivedFromServer);

            server.StreamInput.Value.Should().NotBeEmpty();
            server.StreamInput.EncodedValue.Should().NotBeNullOrWhiteSpace();
        }

        private void StreamInput_ValueChanged(object sender, Core.ValueChangedEventArgs e)
        {
            valueReceivedFromServer.Force(true);
        }
    }
}