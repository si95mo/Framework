using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Mathematics.FuzzyLogic.Tests
{
    [TestFixture]
    public class TestClass
    {
        FuzzySystem fuzzySystem;
        FuzzyVariable service, food, tip;

        [OneTimeSetUp]
        public void Setup()
        {
            fuzzySystem = new FuzzySystem("FuzzySystem");

            service = new FuzzyVariable("Service", 0d, 10d);
            service.AddLinguisticTerm(LinguisticTerm.CreateTriangular("Poor", -5d, 0d, 5d));
            service.AddLinguisticTerm(LinguisticTerm.CreateTriangular("Good", 0d, 5d, 10d));
            service.AddLinguisticTerm(LinguisticTerm.CreateTriangular("Excellent", 5d, 10d, 15d));

            food = new FuzzyVariable("Food", 0d, 10d);
            food.AddLinguisticTerm(LinguisticTerm.CreateTrapezoidal("Rancid", 0d, 0d, 1d, 3d));
            food.AddLinguisticTerm(LinguisticTerm.CreateTrapezoidal("Delicious", 7d, 9d, 10d, 10d));

            tip = new FuzzyVariable("Tip", 0d, 30d, measureUnit: "%", format: "0.0");
            tip.AddLinguisticTerm(LinguisticTerm.CreateTriangular("Cheap", 0d, 5d, 10d));
            tip.AddLinguisticTerm(LinguisticTerm.CreateTriangular("Average", 10d, 15d, 20d));
            tip.AddLinguisticTerm(LinguisticTerm.CreateTriangular("Generous", 20d, 25d, 30d));
        }

        [Test]
        [TestCase(2d, 3d, 0)] // Cheap
        [TestCase(6d, 7d, 1)] // Average
        [TestCase(10d, 9d, 2)] // Generous
        public void Calculate(double serviceValue, double foodValue, int testNumber)
        {
            fuzzySystem.Clear();

            fuzzySystem.AddInput(service, serviceValue);
            fuzzySystem.AddInput(food, foodValue);

            fuzzySystem.AddOutput(tip);

            fuzzySystem.AddRule("if (Service is Poor) or (Food is Rancid) then Tip is Cheap");
            fuzzySystem.AddRule("if (Service is Good) then Tip is Average");
            fuzzySystem.AddRule("if (Service is Excellent) or (Food is Delicious) then (Tip is Generous)");

            List<string> result = fuzzySystem.Calculate();
            result.Should().NotBeEmpty();

            string valueAsString = Regex.Match(result[0], @"\d+\,\d+").Value;
            bool parsed = double.TryParse(valueAsString, out double value);
            parsed.Should().BeTrue();

            switch (testNumber)
            {
                case 0:
                    value.Should().BeLessThan(10d);
                    break;

                case 1:
                    value.Should().BeGreaterThan(10d).And.BeLessThan(20d);
                    break;

                case 2:
                    value.Should().BeGreaterThan(20d).And.BeLessThan(30d);
                    break;
            }
        }
    }
}
