using FluentAssertions;
using NUnit.Framework;

namespace Core.Extensions.Tests
{
    public class ExtensionMethodsTestClass
    {
        private int item;
        private string text;
        bool itWorks = false;

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
            text.With(first, second, third).Should().Be(string.Format(tmp, first, second, third));
        }

        [Test]
        public void BetweenTest()
        {
            item.BetweenInclusive(0, 100).Should().BeTrue();
            item.BetweenExclusive(0, 100).Should().BeTrue();

            item.BetweenInclusive(10, 100).Should().BeTrue();
            item.BetweenExclusive(10, 100).Should().BeFalse();

            item.BetweenInclusive(0, 9).Should().BeFalse();
            item.BetweenExclusive(0, 9).Should().BeFalse();

            item.BetweenInclusive(0, 10).Should().BeTrue();
            item.BetweenExclusive(0, 10).Should().BeFalse();
        }

        [Test]
        public void TestifTrue()
        {

            itWorks = (true.IfTrue(() => true));
            Assert.IsTrue(itWorks);
        }
        [Test]
        public void TestifFalse()
        {
            itWorks = (false.IfFalse(() => true));
            Assert.IsTrue(itWorks);
        }

        [Test]
        public void TestifTrueifFalse()
        {
            itWorks = false.IfTrueIfFalse(() => false, () => true);
            Assert.IsTrue(itWorks);
            itWorks = false;
            itWorks = true.IfTrueIfFalse(() => true, () => false);
            Assert.IsTrue(itWorks);
        }
    }
}
