using Extensions;
using FluentAssertions;
using Hardware;
using Hardware.Resources;
using Hardware.Tcp;
using NUnit.Framework;
using System;
using System.Diagnostics;
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
            AnalogOutput output = new AnalogOutput(Guid.NewGuid().ToString());
            PropertyValueChanged propertyValueChanged = new PropertyValueChanged("ValueChangedCondition", output);
            propertyValueChanged.ValueChanged += PropertyValueChanged_ValueChanged;

            for (int i = 1; i <= 10; i++)
            {
                output.Value = i;
                await Task.Delay(1000);
            }

            eventCounter.Should().Be(10);
        }

        private void PropertyValueChanged_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            eventCounter++;
        }

        [Test]
        [TestCase(1000)]
        [TestCase(10000)]
        public async Task TestTimeElapsed(int timeToWait)
        {
            DummyCondition startCondition = new DummyCondition("StartCondition", false);
            DummyCondition endCondition = new DummyCondition("EndCondition", false);

            TimeElapsedCondition timeElapsedCondition = new TimeElapsedCondition("TimeElapsedCondition", startCondition, endCondition);

            Stopwatch timer = Stopwatch.StartNew();

            startCondition.Force(true);
            await Task.Delay(timeToWait);
            endCondition.Force(true);

            await Task.Delay(10); // A little delay for the condition.Value to be updated
            timer.Stop();

            timeElapsedCondition.Value.Should().BeTrue();
            timeElapsedCondition.ElapsedTime.ValueAsMilliseconds.Should().BeApproximately(timeToWait, 20); // 20ms precision
        }
    }
}