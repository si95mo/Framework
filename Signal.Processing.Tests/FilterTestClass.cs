using Hardware;
using NUnit.Framework;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Signal.Processing.Tests
{
    [TestFixture]
    public class FilterTestClass
    {
        private Filter simpleFilter;
        private Filter filter;

        private AnalogInput input;

        private double fs = 1000; //sampling rate
        private double fw = 5; //signal frequency
        private double fn = 50; //noise frequency
        private double n = 5; //number of periods to show
        private double A = 1; //signal amplitude
        private double N = 0.1; //noise amplitude
        private int size; //sample size

        private double[] t;
        private double[] y;

        [OneTimeSetUp]
        public void Setup()
        {
            size = (int)(n * fs / fw);

            simpleFilter = new Filter(8, 12, 1000);

            input = new AnalogInput("AiInput", "V", "0.000");
            filter = new Filter(8, 12, 1000, input);

            t = Enumerable.Range(1, size).Select(p => p * 1 / fs).ToArray();
            y = t.Select(p => (A * Math.Sin(2 * Math.PI * fw * p)) + (N * Math.Sin(2 * Math.PI * fn * p))).ToArray();

            SaveArray(y, "signal.csv");
        }

        [Test]
        public void FilterSamples()
        {
            double[] filtered = simpleFilter.FilterSamples(y);
            SaveArray(filtered, "simple_log.csv");
        }

        [Test]
        public void FilterIterative()
        {
            double[] filtered = new double[y.Length];
            int i = 0;
            bool updated = false;

            filter.Output.ValueChanged += (object _, Core.ValueChangedEventArgs __) =>
            {
                updated = false;
                filtered[i] = filter.Output.Value;
                updated = true;
            };

            for (; i < y.Length; i++)
            {
                input.Value = y[i];

                do { } while (!updated);
            }

            SaveArray(filtered, "channel_log.csv");
        }

        /// <summary>
        /// Save an array to a text file
        /// </summary>
        /// <param name="a">The array to save</param>
        /// <param name="name">The file name (with extension)</param>
        private void SaveArray(double[] a, string name)
        {
            string str = "";
            a.ToList().ForEach(x => str += $"{x.ToString("F16", CultureInfo.InvariantCulture)}{Environment.NewLine}");

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"test_results//filter_test//{name}");
            string folder = Path.GetDirectoryName(path);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            File.WriteAllText(path, str);
        }
    }
}