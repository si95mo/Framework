using Core.DataStructures;
using FluentAssertions;
using Hardware;
using NUnit.Framework;
using System;

namespace Core.Converters.Calibrations.Tests
{
    [TestFixture]
    public class TestClass
    {
        private AnalogInput rawChannel;
        private AnalogInput calibratedChannel;

        [OneTimeSetUp]
        public void Setup()
        {
            ServiceBroker.Initialize();

            // When raw = 0 -> calibrated = 0; when raw = 10 -> calibrated = 1 => calibrated = raw / 10
            rawChannel = new AnalogInput("RawChannel", "V", "0.000");
            calibratedChannel = Calibrations.CreateCalibratedChannel(rawChannel, 0d, 10d, 0d, 1d, "V", "0.000");
        }

        [Test]
        public void TestCalibration()
        {
            for (int i = short.MinValue; i < short.MaxValue; i++)
            {
                rawChannel.Value = i;
                calibratedChannel.Value.Should().Be(rawChannel.Value / 10d);

                if (i % 10000 == 0)
                    Console.WriteLine($"Raw: {rawChannel} - Calibrated: {calibratedChannel}");
            }
        }
    }
}