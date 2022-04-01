using FluentAssertions;
using Hardware;
using NUnit.Framework;
using System;

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

        [Test]
        [TestCase(10)]
        public void AddTags(int numberOfTags)
        {
            AnalogInput channel = new AnalogInput(Guid.NewGuid().ToString());

            for (int i = 1; i <= numberOfTags; i++)
                channel.Tags.Add(i.ToString());

            channel.Tags.Count.Should().Be(numberOfTags);

            for (int i = 1; i <= numberOfTags; i++)
                channel.Tags[i - 1].Should().BeEquivalentTo(i.ToString());
        }
    }
}