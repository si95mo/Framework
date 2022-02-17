using Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace Core.Conditions.Tests
{
    [TestFixture]
    public class TestClass
    {
        private ConditionFlyweight mainCondition;

        private readonly ConditionFlyweight falseCondition = new ConditionFlyweight("FalseCondition", false);
        private readonly ConditionFlyweight trueCondition = new ConditionFlyweight("TrueCondition", true);

        [OneTimeSetUp]
        public void Setup()
        {
            mainCondition = new ConditionFlyweight("MainCondition", true);

            mainCondition.IsFalse().Value.Should().Be(false);
            mainCondition.IsTrue().Value.Should().Be(true);

            mainCondition.Value = false;
            mainCondition.IsFalse().Value.Should().Be(true);
            mainCondition.IsTrue().Value.Should().Be(false);
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
    }
}
