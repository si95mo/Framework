using FluentAssertions;
using NUnit.Framework;

namespace Extensions.Tests
{
    public class ExtensionMethodsTestClass
    {
        private int item;
        private string text;
        private bool itWorks = false;

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
        public void TestifTrue()
        {
            itWorks = true.DoIfTrue(() => true);
            itWorks.Should().BeTrue();
        }

        [Test]
        public void TestifFalse()
        {
            itWorks = false.DoIfFalse(() => true);
            itWorks.Should().BeTrue();
        }

        [Test]
        public void TestifTrueifFalse()
        {
            itWorks = false.DoIfTrueIfFalse(() => false, () => true);
            itWorks.Should().BeTrue();
            itWorks = false;
            itWorks = true.DoIfTrueIfFalse(() => true, () => false);
            itWorks.Should().BeTrue();
        }
    }
}