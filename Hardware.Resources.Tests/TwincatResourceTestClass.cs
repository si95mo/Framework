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
        private string amsNetAddress = "1.1.1.1.1.1"; // To check which is correct for the local ads server
        // private int port = 12345; // Mock server - only this parameter in the constructor of the resource
        private int port = 851; // TwinCAT - local ads server
        private TwincatResource resource;
        private Mock server;

        [OneTimeSetUp]
        public void Setup()
        {
            // The mock server only specify a valid port number
            // resource = new TwincatResource("TwincatResource", port);
            resource = new TwincatResource("TwincatResource", amsNetAddress, port);
        }

        [Test]
        public async Task StartTest()
        {
            await resource.Start();
            resource.Status.Value.Should().Be(ResourceStatus.Executing);
        }
    }
}