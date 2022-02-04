using Hardware.WaveformGenerator;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Hardware.Resources.Tests
{
    [TestFixture]
    public class WaveformGeneratorResourceTestClass
    {
        private WaveformGeneratorResource resource;
        private AnalogOutput output;

        [OneTimeSetUp]
        public void Setup()
        {
            output = new AnalogOutput("WaveformOutput", measureUnit: "V", format: "0.000000000000");
        }

        [Test]
        [TestCase(WaveformType.Sine)]
        [TestCase(WaveformType.Square)]
        [TestCase(WaveformType.Triangular)]
        [TestCase(WaveformType.Sawtooth)]
        public async Task Test(WaveformType type)
        {
            resource = new WaveformGeneratorResource("WaveformResource", type, 1, 10, output);
            await resource.Start();

            string str = "";
            bool update = true;

            output.ValueChanged += (object sender, Core.ValueChangedEventArgs e) =>
            {
                if (update)
                    str += $"{output.Value.ToString("F16", CultureInfo.InvariantCulture)}{Environment.NewLine}";
            };
            Stopwatch sw = Stopwatch.StartNew();

            do
            {
                await Task.Delay(0);
            } while (sw.Elapsed.TotalMilliseconds <= 2000); // 10 seconds
            update = false;

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"test_results//waveform_test//{type}_test.csv");
            string folder = Path.GetDirectoryName(path);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            File.WriteAllText(path, str);
        }
    }
}