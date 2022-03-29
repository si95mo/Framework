using Core.Parameters;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace Core.DataStructures.Tests
{
    internal enum DummyEnum
    {
        None = 0,
        First = 1,
        Second = 2,
        Third = 3
    }

    internal class ParameterTestClass
    {
        private NumericParameter firstNumericParameter, secondNumericParameter;
        private BoolParameter firstBooleanParameter, secondBooleanParameter;
        private StringParameter firstStringParameter, secondStringParameter;
        private TimeSpanParameter timeSpanParameter;
        private EnumParameter<DummyEnum> enumParameter;

        private bool eventFired = false;

        [OneTimeSetUp]
        public void Setup()
        {
            firstNumericParameter = new NumericParameter(nameof(firstNumericParameter));
            secondNumericParameter = new NumericParameter(nameof(secondNumericParameter));

            firstBooleanParameter = new BoolParameter(nameof(firstBooleanParameter));
            secondBooleanParameter = new BoolParameter(nameof(secondBooleanParameter));

            firstStringParameter = new StringParameter(nameof(firstStringParameter));
            secondStringParameter = new StringParameter(nameof(secondStringParameter));

            timeSpanParameter = new TimeSpanParameter(nameof(timeSpanParameter));

            enumParameter = new EnumParameter<DummyEnum>(nameof(enumParameter));

            firstNumericParameter.ConnectTo(secondNumericParameter);
            firstBooleanParameter.ConnectTo(secondBooleanParameter);
            firstStringParameter.ConnectTo(secondStringParameter);

            ServiceBroker.Initialize();
            ServiceBroker.Add<IParameter>(firstNumericParameter);
            ServiceBroker.Add<IParameter>(secondNumericParameter);
            ServiceBroker.Add<IParameter>(firstBooleanParameter);
            ServiceBroker.Add<IParameter>(secondBooleanParameter);
            ServiceBroker.Add<IParameter>(firstStringParameter);
            ServiceBroker.Add<IParameter>(secondStringParameter);
            ServiceBroker.Add<IParameter>(timeSpanParameter);
            ServiceBroker.Add<IParameter>(enumParameter);
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
            firstBooleanParameter.ValueChanged += FirstBooleanParameter_ValueChanged;

            firstNumericParameter.Value = -1.5;
            firstNumericParameter.MeasureUnit = "V";

            string toString = firstNumericParameter.ToString();
            (toString.CompareTo("-1.5V") == 0 || toString.CompareTo("-1,5V") == 0).Should().BeTrue();
            secondNumericParameter.Value.Should().Be(firstNumericParameter.Value);

            firstBooleanParameter.Value = true;
            secondBooleanParameter.Value.Should().Be(firstBooleanParameter.Value);

            firstStringParameter.Value = "Hello world!";
            secondStringParameter.Value.Should().Be(firstStringParameter.Value);

            TimeSpan span = new TimeSpan(0, 0, 0, 1, 1000);
            timeSpanParameter.Value = span;
            timeSpanParameter.Value.Should().Be(span);

            foreach (var item in Enum.GetValues(typeof(DummyEnum)))
            {
                enumParameter.Value = (DummyEnum)item;
                enumParameter.Value.Should().Be((DummyEnum)item);
            }

            eventFired.Should().BeTrue();
        }

        private void FirstBooleanParameter_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            eventFired = true;
        }
    }
}