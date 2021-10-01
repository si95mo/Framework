using FluentAssertions;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Core.Threading.Tests
{
    public class NoOperationTestClass
    {
        [Test]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(1000)]
        public async Task DoNoOperation(int interval)
        {
            double threshold = 16d; // ms

            Stopwatch sw = Stopwatch.StartNew();
            await Tasks.NoOperation(interval);
            sw.Stop();

            Math.Abs(sw.Elapsed.TotalMilliseconds - interval).Should().BeLessThan(threshold);

            sw = Stopwatch.StartNew();
            await Tasks.NoOperation(interval, 1);
            sw.Stop();

            Math.Abs(sw.Elapsed.TotalMilliseconds - interval).Should().BeLessThan(threshold);
        }
    }
}