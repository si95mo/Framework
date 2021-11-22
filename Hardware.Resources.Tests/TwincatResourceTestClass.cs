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
        private string amsNetAddress = "LocalAddress";
        private int port = 801;
        private TwincatResource resource;
        Mock server;

        [OneTimeSetUp]
        public async Task Setup()
        {
            server = new Mock((ushort)port, amsNetAddress);

            resource = new TwincatResource("TwincatResource", amsNetAddress, port);
            await resource.Start();

            resource.Status.Value.Should().Be(ResourceStatus.Executing);
        }

        [Test]
        public void Test()
        {

        }
    }
}
