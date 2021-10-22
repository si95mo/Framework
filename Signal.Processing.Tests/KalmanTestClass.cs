using Core.Converters;
using FluentAssertions;
using Hardware;
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
        private Kalman filteredKalman;
        private Kalman rawKalman;

        private AnalogInput x = new AnalogInput("X", format: "0.000");
        private AnalogInput lowPassFiltered = new AnalogInput("Xf", format: "0.000");

        private List<double> data;
        private List<double> filteredData;

        [OneTimeSetUp]
        public void Setup()
        {
            x.ConnectTo(lowPassFiltered, new SimpleMovingAverage(4));
            filteredKalman = new Kalman(
                1, // A
                1, // H
                2.8, // Q
                1.7, // R
                0.5, // P
                lowPassFiltered // X
            );
            rawKalman = new Kalman(
                1, // A
                1, // H
                2.8, // Q
                1.7, // R
                0.5, // P
                x // X
            );

            data = new List<double>();
            filteredData = new List<double>();
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

            do
            {
                await GenerateSignal(t.Elapsed.TotalMilliseconds);
            } while (t.Elapsed.TotalMilliseconds <= 1000); // 10s

            using (StreamWriter writer = File.CreateText(path))
            {
                uint counter = 0;
                for (int i = 0; i < data.Count; i++)
                {
                    text = $"{data.ElementAt(i).ToString().Replace(',', '.')}; " +
                        $"{filteredData.ElementAt(i).ToString().Replace(',', '.')}; " +
                        $"{filteredKalman.Filtered.ElementAt(i).ToString().Replace(',', '.')}; " +
                        $"{rawKalman.Filtered.ElementAt(i).ToString().Replace(',', '.')}";
                    writer.WriteLine(text);

                    if (data.ElementAt(i) == filteredKalman.Filtered.ElementAt(i))
                        counter++;
                }

                counter.Should().BeLessOrEqualTo((uint)(data.Count / 100)); // Different values
            };
        }

        private async Task GenerateSignal(double t)
        {
            bool sign = new Random(Guid.NewGuid().GetHashCode()).NextDouble() > 0.5;
            double noise = new Random(Guid.NewGuid().GetHashCode()).NextDouble() / 10d; // 10%

            if (sign) // Positive
                x.Value = Math.Sin(t) + noise;
            else // Negative
                x.Value = Math.Sin(t) - noise;

            data.Add(x.Value);
            filteredData.Add(lowPassFiltered.Value);

            await Task.Delay(10);
        }
    }
}