using Extensions;
using FluentAssertions;
using Hardware.Resources;
using Hardware.Tcp;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Core.Conditions.Tests
{
    [TestFixture]
    public class TestClass
    {
        private FlyweightCondition mainCondition;

        private readonly FlyweightCondition falseCondition = new FlyweightCondition("FalseCondition", false);
        private readonly FlyweightCondition trueCondition = new FlyweightCondition("TrueCondition", true);

        private TcpResource resource; // Simulated tcp server
        private TcpChannel channel;
        private int eventCounter = 0;

        /// <summary>
        /// Get the host PC local ip address
        /// </summary>
        /// <returns>The local ip address</returns>
        private string GetLocalIp()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();

            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        [OneTimeSetUp]
        public async Task Setup()
        {
            mainCondition = new FlyweightCondition("MainCondition", true);

            mainCondition.IsFalse().Value.Should().Be(false);
            mainCondition.IsTrue().Value.Should().Be(true);

            mainCondition.Value = false;
            mainCondition.IsFalse().Value.Should().Be(true);
            mainCondition.IsTrue().Value.Should().Be(false);

            resource = new TcpResource("TcpResource", GetLocalIp(), 10000, 5000);
            channel = new TcpChannel("Channel", resource);

            await resource.Start();
        }

        [Test]
        public void TestAnd()
        {
            mainCondition.Value = true;

            ICondition result = mainCondition.And(falseCondition);
            result.Value.Should().BeFalse();

            result = mainCondition.And(trueCondition);
            result.Value.Should().BeTrue();
        }

        [Test]
        public void TestOr()
        {
            mainCondition.Value = false;

            ICondition result = mainCondition.Or(falseCondition);
            result.Value.Should().BeFalse();

            result = mainCondition.Or(trueCondition);
            result.Value.Should().BeTrue();
        }

        [Test]
        public async Task TestPropertyValueChanged()
        {
            PropertyValueChanged propertyValueChanged = new PropertyValueChanged("ValueChangedCondition", channel);
            propertyValueChanged.ValueChanged += PropertyValueChanged_ValueChanged;

            for (int i = 0; i < 10; i++)
            {
                channel.Request = i.ToString();
                await Task.Delay(10000);
            }

            eventCounter.Should().BeGreaterThan(9);
        }

        private void PropertyValueChanged_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            eventCounter++;
        }
    }
}