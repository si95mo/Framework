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
    internal class MovingAverageTestClass
    {
        public AnalogInput ai = new AnalogInput("AnalogInput");
        public NumericParameter npSimple = new NumericParameter("NumericParameterSimple");
        public NumericParameter npExponential = new NumericParameter("NumericParameterExponential");

        [OneTimeSetUp]
        public void Setup()
        {
            npSimple.Value = 0;
            ai.ConnectTo(npSimple, new SimpleMovingAverage(8));
            ai.ConnectTo(npExponential, new ExponentialMovingAverage(0.75));

            ServiceBroker.Init();
            ServiceBroker.Add<IChannel>(ai);
            ServiceBroker.Add<IParameter>(npSimple);
        }

        [OneTimeTearDown]
        public void Dispose()
        {
            ServiceBroker.Clear();
        }

        [Test]
        [TestCase(1000, false)]
        public void TestSimpleMovingAverage(int interval, bool saveFiles)
        {
            Stopwatch sw = Stopwatch.StartNew();
            do
            {
                int sign = new Random(Guid.NewGuid().GetHashCode()).NextDouble() > 0.5 ? 1 : -1;
                double number = 10 + sign * new Random(Guid.NewGuid().GetHashCode()).NextDouble();
                ai.Value = number;
                npSimple.Value.Should().NotBe(0);

                if (saveFiles)
                {
                    FileHandler.Save(
                        $"{ai.Value:F6}", IOUtility.GetDesktopFolder() + @"\raw_simple.log",
                        FileHandler.SaveMode.Append
                    );
                    FileHandler.Save(
                        $"{npSimple.Value:F6}", IOUtility.GetDesktopFolder() + @"\filtered_simple.log",
                        FileHandler.SaveMode.Append
                    );
                }
            } while (sw.Elapsed.TotalMilliseconds <= interval);
        }

        [Test]
        [TestCase(1000, false)]
        public void TestExponentialMovingAverage(int interval, bool saveFiles)
        {
            Stopwatch sw = Stopwatch.StartNew();
            do
            {
                int sign = new Random(Guid.NewGuid().GetHashCode()).NextDouble() > 0.5 ? 1 : -1;
                double number = 10 + sign * new Random(Guid.NewGuid().GetHashCode()).NextDouble();
                ai.Value = number;
                npExponential.Value.Should().NotBe(0);

                if (saveFiles)
                {
                    FileHandler.Save(
                        $"{ai.Value:F6}", IOUtility.GetDesktopFolder() + @"\raw_exp.log",
                        FileHandler.SaveMode.Append
                    );
                    FileHandler.Save(
                        $"{npExponential.Value:F6}", IOUtility.GetDesktopFolder() + @"\filtered_exp.log",
                        FileHandler.SaveMode.Append
                    );
                }
            } while (sw.Elapsed.TotalMilliseconds <= interval);
        }
    }
}