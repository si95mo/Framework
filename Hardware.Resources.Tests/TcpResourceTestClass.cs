﻿using FluentAssertions;
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
        private TcpInput input;
        private TcpOutput output;

        [OneTimeSetUp]
        public async Task Setup()
        {
            resource = new TcpResource("TcpResource", GetLocalIp(), 10000);

            await resource.Start();
            resource.Status.Should().Be(ResourceStatus.Executing);

            input = new TcpInput("TcpInput", $"Hello IN {Environment.NewLine}", resource, 100);
            output = new TcpOutput("TcpOutput", $"Hello OUT {Environment.NewLine}", resource);
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
            resource.Send($"{message} (Send){Environment.NewLine}");

            resource.SendAndReceive(
                $"{message} (SendAndReceive){Environment.NewLine}",
                out string response
            );
            response.Should().NotBe(""); // The response is sent back from Hercules

            for (int i = 1; i <= 4; i++)
            {
                output.Value = i.ToString();
                await Task.Delay(2000);
                Console.WriteLine($"{DateTime.Now:ss:ffff} - {input.Value} ({i})");
            }
        }
    }
}