using FluentAssertions;
using NUnit.Framework;

namespace Core.Extensions.Tests
{
    public class ExtensionMethodsTestClass
    {
        private int item;
        private string text;

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
        public void FormatWith(string first, string second, string third)
        {
            string tmp = text;
            text.With(first, second, third).Should().Be(string.Format(tmp, first, second, third));
        }
    }
}
