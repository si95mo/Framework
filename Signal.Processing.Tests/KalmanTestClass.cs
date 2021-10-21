using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Signal.Processing.Tests
{
    [TestFixture]
    public class KalmanTestClass
    {
        private Kalman kalman;
        private List<double> x;

        [OneTimeSetUp]
        public void Setup()
        {
            kalman = new Kalman(1, 1, 0.125, 1, 0.1, 0d);
            x = new List<double>();
        }

        [Test]
        public async Task Test()
        {
            string text;
            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "kalman_logs",
                "kalman_log.csv"
            );

            Stopwatch t = Stopwatch.StartNew();
            double data;
            bool sign;
            double noise;

            do
            {
                sign = new Random(Guid.NewGuid().GetHashCode()).NextDouble() > 0.5;

                if (sign)
                    data = Math.Sin(t.Elapsed.TotalMilliseconds) +
                        (new Random(Guid.NewGuid().GetHashCode()).NextDouble() / 100d);
                else
                    data = Math.Sin(t.Elapsed.TotalMilliseconds) -
                        (new Random(Guid.NewGuid().GetHashCode()).NextDouble() / 100d);

                x.Add(data);

                await Task.Delay(10);
            } while (t.Elapsed.TotalMilliseconds <= 10000);

            List<double> filteredData = new List<double>();
            using (StreamWriter writer = File.CreateText(path))
            {
                double filtered;
                for (int i = 0; i < x.Count; i++)
                {
                    filtered = kalman.Filter(x.ElementAt(i));
                    filteredData.Add(filtered);

                    text = $"{x.ElementAt(i).ToString().Replace(',', '.')}; " +
                        $"{filtered.ToString().Replace(',', '.')}";
                    writer.WriteLine(text);
                }
            };

            filteredData.Should().NotBeEquivalentTo(x);
        }
    }
}