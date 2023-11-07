using Extensions;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace Engines.Tests
{
    [TestFixture]
    public class EnginesTestClass
    {
        private JavascriptEngine engine;

        [OneTimeSetUp]
        public void Setup()
        {
            engine = new JavascriptEngine();
        }

        [Test]
        [TestCase("1 + 2", typeof(int), 3)]
        [TestCase("1.2 + 4.7", typeof(double), 5.9)]
        [TestCase("1 > 2", typeof(bool), false)]
        [TestCase("\"01234\" + \"_\" + \"56789\"", typeof(string), "01234_56789")]
        public void Test(string script, Type type, object expected)
        {
            object result = null;
            if (type.IsNumeric())
                result = engine.ExecuteAsDouble(script);
            else if (type == typeof(bool))
                result = engine.ExecuteAsBool(script);
            else if (type == typeof(string))
                result = engine.ExecuteAsString(script);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }
    }
}