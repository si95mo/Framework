using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Hardware.Resources.Tests
{
    public class TcpResourceTestClass
    {
        private TcpResource resource;

        [OneTimeSetUp]
        public void Setup()
        {
            resource = new TcpResource("TcpResource", GetLocalIp(), 10000);

            resource.Start();
            resource.Status.Should().Be(ResourceStatus.Executing);
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
        public void Test(string message)
        {
            resource.Send(message + " (Send)\n");

            resource.SendAndReceive(message + " (SendAndReceive)\n", out string response);
            response.Should().Be("Hello!"); // The response is sent back from Hercules
        }
    }
}
