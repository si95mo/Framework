using dsian.TwinCAT.Ads.Server.Mock;
using FluentAssertions;
using Hardware.Twincat;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Hardware.Resources.Tests
{
    [TestFixture]
    public class TwincatResourceTestClass
    {
        private string amsNetAddress = "MyTestAdsServer";
        private int port = 12345;
        private TwincatResource resource;
        private Mock server;

        [OneTimeSetUp]
        public void Setup()
        {
            // server = new Mock((ushort)port, amsNetAddress);
            // The mock server only specify a valid port number
            resource = new TwincatResource("TwincatResource", port);
        }

        [Test]
        public async Task StartTest()
        {
            await resource.Start();
            resource.Status.Value.Should().Be(ResourceStatus.Executing);
        }
    }
}