using Core.Parameters;
using FluentAssertions;
using NUnit.Framework;

namespace Core.DataStructures.Tests
{
    class ParameterTestClass
    {
        private NumericParameter firstNumericParameter, secondNumericParameter;
        private BooleanParameter firstBooleanParameter, secondBooleanParameter;
        private StringParameter firstStringParameter, secondStringParameter;

        [OneTimeSetUp]
        public void Setup()
        {
            firstNumericParameter = new NumericParameter(nameof(firstNumericParameter));
            secondNumericParameter = new NumericParameter(nameof(secondNumericParameter));

            firstBooleanParameter = new BooleanParameter(nameof(firstBooleanParameter));
            secondBooleanParameter = new BooleanParameter(nameof(secondBooleanParameter));

            firstStringParameter = new StringParameter(nameof(firstStringParameter));
            secondStringParameter = new StringParameter(nameof(secondStringParameter));

            firstNumericParameter.ConnectTo(secondNumericParameter);
            firstBooleanParameter.ConnectTo(secondBooleanParameter);
            firstStringParameter.ConnectTo(secondStringParameter);

            ServiceBroker.Init();
            ServiceBroker.Add<IParameter>(firstNumericParameter);
            ServiceBroker.Add<IParameter>(secondNumericParameter);
            ServiceBroker.Add<IParameter>(firstBooleanParameter);
            ServiceBroker.Add<IParameter>(secondBooleanParameter);
            ServiceBroker.Add<IParameter>(firstStringParameter);
            ServiceBroker.Add<IParameter>(secondStringParameter);
        }

        [OneTimeTearDown]
        public void Dispose()
        {
            ServiceBroker.Clear();
            ServiceBroker.Get<IParameter>().Count.Should().Be(0);
        }

        [Test]
        public void ParameterTest()
        {
            firstNumericParameter.Value = -1.5;
            firstNumericParameter.MeasureUnit = "V";

            string toString = firstNumericParameter.ToString();
            (toString.CompareTo("-1.5V") == 0 || toString.CompareTo("-1,5V") == 0).Should().BeTrue();
            secondNumericParameter.Value.Should().Be(firstNumericParameter.Value);

            firstBooleanParameter.Value = true;
            secondBooleanParameter.Value.Should().Be(firstBooleanParameter.Value);

            firstStringParameter.Value = "Hello world!";
            secondStringParameter.Value.Should().Be(firstStringParameter.Value);
        }
    }
}
