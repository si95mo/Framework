using Core.DataStructures;
using Core.Parameters;
using FluentAssertions;
using Hardware;
using IO;
using NUnit.Framework;
using System;
using System.Diagnostics;

namespace Core.Converters.Tests
{
    class MovingAverageTestClass
    {
        public AnalogInput ai = new AnalogInput("AnalogInput");
        public NumericParameter np = new NumericParameter("NumericParameter");

        [OneTimeSetUp]
        public void Setup()
        {
            np.Value = 0;
            ai.ConnectTo(np, new SimpleMovingAverage(8));

            ServiceBroker.Init();
            ServiceBroker.Add<IChannel>(ai);
            ServiceBroker.Add<IParameter>(np);
        }

        [OneTimeTearDown]
        public void Dispose()
        {
            ServiceBroker.Clear();
        }

        [Test]
        [TestCase(1000)]
        public void TestMovingAverage(int interval)
        {
            Stopwatch sw = Stopwatch.StartNew();
            do
            {
                double number = 10 + new Random(Guid.NewGuid().GetHashCode()).NextDouble();
                ai.Value = number;
                np.Value.Should().NotBe(0);

                FileHandler.Save(
                    $"{ai.Value:F6}", IOUtility.GetDesktopFolder() + @"\raw.log", 
                    FileHandler.MODE.Append
                );
                FileHandler.Save(
                    $"{np.Value:F6}", IOUtility.GetDesktopFolder() + @"\filtered.log",
                    FileHandler.MODE.Append
                );
            } while (sw.Elapsed.TotalMilliseconds <= interval);
        }
    }
}
