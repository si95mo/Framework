using FluentAssertions;
using Hardware.Tcp;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Hardware.Resources.Tests
{
    public class TcpResourceTestClass
    {
        private TcpResource resource;
        private TcpChannel channel;

        [OneTimeSetUp]
        public async Task Setup()
        {
            resource = new TcpResource("TcpResource", GetLocalIp(), 10000);

            await resource.Start();
            resource.Status.Value.Should().Be(ResourceStatus.Executing);

            channel = new TcpChannel("TcpInput", "", resource, usePolling: false);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            resource.Stop();
            resource.Status.Should().Be(ResourceStatus.Stopped);
        }

        private string GetLocalIp()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();

            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        [Test]
        [TestCase("Hello, world!")]
        public async Task Test(string message)
        {
            channel.Request = message + Environment.NewLine;

            await Task.Delay(2000);

            channel.Response.Should().NotBe(""); // The response is sent back from Hercules
        }
    }
}