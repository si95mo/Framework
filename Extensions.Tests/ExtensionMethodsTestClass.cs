using Core;
using Core.Conditions;
using FluentAssertions;
using Hardware.Resources;
using Hardware.Tcp;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Extensions.Tests
{
    public class ExtensionMethodsTestClass
    {
        private int item;
        private string text;
        private bool itWorks = false;
        private int eventCounter = 0;

        [OneTimeSetUp]
        public void Setup()
        {
            item = 10;
            text = "First: {0}. Second: {1}. Third: {2}";
        }

        [Test]
        public void IsInTest()
        {
            item.IsIn(0, 1, 2, 3, 4, 5, 6, 7, 8, 9).Should().BeFalse();
            item.IsIn(10, 11, 12, 13).Should().BeTrue();
        }

        [Test]
        [TestCase("A", "B", "C")]
        public void FormatWithTest(string first, string second, string third)
        {
            string tmp = text;
            text.FormatWith(first, second, third).Should().Be(string.Format(tmp, first, second, third));
        }

        [Test]
        public void BetweenTest()
        {
            item.IsBetweenInclusive(0, 100).Should().BeTrue();
            item.IsBetweenExclusive(0, 100).Should().BeTrue();

            item.IsBetweenInclusive(10, 100).Should().BeTrue();
            item.IsBetweenExclusive(10, 100).Should().BeFalse();

            item.IsBetweenInclusive(0, 9).Should().BeFalse();
            item.IsBetweenExclusive(0, 9).Should().BeFalse();

            item.IsBetweenInclusive(0, 10).Should().BeTrue();
            item.IsBetweenExclusive(0, 10).Should().BeFalse();
        }

        [Test]
        public void TestIfTrue()
        {
            itWorks = true.DoIfTrue(() => true);
            itWorks.Should().BeTrue();
        }

        [Test]
        public void TestIfFalse()
        {
            itWorks = false.DoIfFalse(() => true);
            itWorks.Should().BeTrue();
        }

        [Test]
        public void TestifTrueIfFalse()
        {
            itWorks = false.DoIfTrueIfFalse(() => false, () => true);
            itWorks.Should().BeTrue();
            itWorks = false;
            itWorks = true.DoIfTrueIfFalse(() => true, () => false);
            itWorks.Should().BeTrue();
        }

        [Test]
        [TestCase(1000)]
        public void TestTimedWhile(int interval)
        {
            int counter = 0;
            int n = 10;

            // Action that also concur to the condition handling
            Action source = () => Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} >> Counter value: {counter++}");
            Func<bool> condition = new Func<bool>(() => TestCondition(counter, n));

            Stopwatch sw = Stopwatch.StartNew();
            source.TimedWhile(condition, interval); // Or: source.TimedWhile(() => counter < n, interval);
            sw.Stop();

            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} >> Time taken: {sw.Elapsed.TotalMilliseconds:0.0}[ms]");
            sw.Elapsed.TotalMilliseconds.Should().BeApproximately(interval * n, 100);
        }

        private bool TestCondition(int counter, int n) => counter < n;

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

        [Test]
        public async Task TestWaitFor()
        {
            TcpResource resource = new TcpResource("TcpResource", GetLocalIp(), 10000, 5000);
            TcpChannel channel = new TcpChannel("Channel", resource);

            await resource.Start();

            PropertyValueChanged propertyValueChanged = new PropertyValueChanged("ValueChangedCondition", channel);
            propertyValueChanged.ValueChanged += PropertyValueChanged_ValueChanged;

            for (int i = 0; i < 10; i++)
            {
                channel.Request = i.ToString();
                await this.WaitFor(propertyValueChanged, 5000);
            }
        }

        private void PropertyValueChanged_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            eventCounter++;
        }
    }
}