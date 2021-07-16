using FluentAssertions;
using NUnit.Framework;
using System;
using System.Diagnostics;

namespace Core.Threading.Tests
{
    public class NoOperationTestClass
    {
        [Test]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(1000)]
        public void DoNoOperation(int interval)
        {
            double threshold = 15d; // ms

            Stopwatch sw = Stopwatch.StartNew();
            Tasks.NoOperation(interval);
            sw.Stop();

            Math.Abs(sw.Elapsed.TotalMilliseconds - interval).Should().BeLessThan(threshold);

            threshold = 5d; // ms

            sw = Stopwatch.StartNew();
            Tasks.NoOperation(interval, 1);
            sw.Stop();

            Math.Abs(sw.Elapsed.TotalMilliseconds - interval).Should().BeLessThan(threshold);
        }
    }
}