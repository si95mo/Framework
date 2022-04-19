using Core.Conditions;
using Extensions;
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

        private bool eventFired;

        [OneTimeSetUp]
        public async Task Setup()
        {
            resource = new TcpResource("TcpResource", GetLocalIp(), 10000);

            await resource.Start();
            resource.Status.Value.Should().Be(ResourceStatus.Executing);

            channel = new TcpChannel("TcpInput", "", resource, usePolling: false);

            eventFired = false;
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
            channel.ValueChanged += Channel_ValueChanged;
            channel.Request = message + Environment.NewLine;
            PropertyValueChanged condition = new PropertyValueChanged(Guid.NewGuid().ToString(), channel);
            condition.ValueChanged += Condition_ValueChanged;

            System.Diagnostics.Stopwatch timer = System.Diagnostics.Stopwatch.StartNew();
            await this.WaitFor(condition, 5000);
            timer.Stop();

            channel.Value.Should().NotBe(""); // The response is sent back from Hercules (manual)
            timer.Elapsed.TotalMilliseconds.Should().BeLessThan(5000);
        }

        private void Condition_ValueChanged(object sender, Core.ValueChangedEventArgs e)
        {
            eventFired = true;
        }

        private void Channel_ValueChanged(object sender, Core.ValueChangedEventArgs e)
        {
            eventFired = true;
        }
    }
}