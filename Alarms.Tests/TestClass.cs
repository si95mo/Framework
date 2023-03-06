using Core.Conditions;
using Core.Parameters;
using FluentAssertions.Numeric;
using NUnit.Framework;
using System;
using Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Alarms.Tests
{
    [TestFixture]
    public class TestClass
    {
        private Alarm alarm;
        private DummyCondition condition;
        private NumericParameter parameter;

        private ICondition conditionIsTrue;

        [OneTimeSetUp]
        public void Setup()
        {
            parameter = new NumericParameter();
            condition = new DummyCondition("InitialCondition", false);

            alarm = Alarm.New("Alarm", parameter, "Message", condition.IsTrue());

            conditionIsTrue = condition.IsTrue();

            condition.ValueChanged += Condition_ValueChanged;
            conditionIsTrue.ValueChanged += Condition_ValueChanged;
        }

        private void Condition_ValueChanged(object sender, Core.ValueChangedEventArgs e)
        {
            Console.WriteLine($"{(sender as ICondition).Code} valuechanged event handler fired");
        }

        [Test]
        public async Task Test()
        {
            condition.Force(false);
            await Task.Delay(1000);
            condition.Force(true);

            DateTime now = DateTime.Now;

            alarm.FiringTime.Ticks.Should().BeLessThanOrEqualTo(now.Ticks);
        }
    }
}
