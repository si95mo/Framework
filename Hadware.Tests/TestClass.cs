using FluentAssertions;
using Hardware;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hadware.Tests
{
    public class HardwareTest
    {
        private AnalogOutput sourceAnalogChannel;
        private AnalogInput destinationAnalogChannel;

        private DigitalOutput sourceDigitalChannel;
        private DigitalInput destinationDigitalChannel;

        [OneTimeSetUp]
        public void Init()
        {
            sourceAnalogChannel = new AnalogOutput(nameof(sourceAnalogChannel));
            destinationAnalogChannel = new AnalogInput(nameof(destinationAnalogChannel));

            sourceDigitalChannel = new DigitalOutput(nameof(sourceDigitalChannel));
            destinationDigitalChannel = new DigitalInput(nameof(destinationDigitalChannel));
        }

        [Test]
        [TestCase(1.0)]
        public void ConnectToAnalogChannelTest(double value)
        {
            sourceAnalogChannel.Value = value;
            sourceAnalogChannel.ConnectTo(destinationAnalogChannel);

            destinationAnalogChannel.Value.Should().Be(value);

            double newValue = 2 * value;
            sourceAnalogChannel.Value = newValue;

            destinationAnalogChannel.Value.Should().Be(newValue);
        }

        [Test]
        [TestCase(true)]
        public void ConnectToDigitalChannelTest(bool value)
        {
            sourceDigitalChannel.Value = value;
            sourceDigitalChannel.ConnectTo(destinationDigitalChannel);

            destinationDigitalChannel.Value.Should().Be(value);

            bool newValue = !value;
            sourceDigitalChannel.Value = newValue;

            destinationDigitalChannel.Value.Should().Be(newValue);
        }
    }
}
