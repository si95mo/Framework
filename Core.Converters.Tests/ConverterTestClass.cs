using Core.DataStructures;
using Core.Parameters;
using FluentAssertions;
using Hardware;
using NUnit.Framework;
using System;

namespace Core.Converters.Tests
{
    public class ConverterTestClass
    {
        private double conversionFactor = 0.5;

        private AnalogInput ai = new AnalogInput("AnalogInput");
        private AnalogOutput ao = new AnalogOutput("AnalogOutput");
        private NumericParameter np = new NumericParameter("NumericParameter");

        [OneTimeSetUp]
        public void Setup()
        {
            ai.ConnectTo(
                np,
                new GenericConverter<double, double>(
                    (x) => x * conversionFactor
                )
            );
            ai.ConnectTo(ao);

            ServiceBroker.Initialize();
            ServiceBroker.Add<IChannel>(ai);
            ServiceBroker.Add<IParameter>(np);
        }

        [OneTimeTearDown]
        public void Dispose()
        {
            ServiceBroker.Clear();
        }

        [Test]
        [TestCase(1.0)]
        [TestCase(Math.PI)]
        [TestCase(Math.E)]
        [TestCase(-1.2345)]
        public void TestConverter(double value)
        {
            ai.Value = value;
            np.Value.Should().Be(ai.Value * conversionFactor);
            ao.Value.Should().Be(ai.Value);
        }
    }
}