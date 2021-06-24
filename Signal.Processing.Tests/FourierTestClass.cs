using FluentAssertions;
using Mathematics;
using Microsoft.VisualBasic.FileIO;
using NUnit.Framework;
using Signal.Processing.Tests.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Signal.Processing.Tests
{
    public class FourierTestClass
    {
        private const int N = 1024;
        private double[] signal;
        private NumberFormatInfo nfi;
        double[] samples;

        [OneTimeSetUp]
        public void Setup()
        {
            signal = new double[N];

            nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";

        }

        private double BytesToDouble(byte firstByte, byte secondByte)
        {
            // convert two bytes to one short (little endian)
            short s = (short)((secondByte << 8) | firstByte);
            // convert to range from -1 to (just below) 1
            return s / 32768.0;
        }

        private double[] LoadResourceAndAddOffset(UnmanagedMemoryStream stream, double offset)
        {
            double[] samples = LoadResource(stream);

            for (int i = 0; i < samples.Length; i++)
                samples[i] += offset;

            return samples;
        }

        private double[] LoadResource(UnmanagedMemoryStream stream)
        {
            BinaryReader reader = new BinaryReader(stream);

            //Read the wave file header from the buffer. 

            int chunkID = reader.ReadInt32();
            int fileSize = reader.ReadInt32();
            int riffType = reader.ReadInt32();
            int fmtID = reader.ReadInt32();
            int fmtSize = reader.ReadInt32();
            int fmtCode = reader.ReadInt16();
            int channels = reader.ReadInt16();
            int sampleRate = reader.ReadInt32();
            int fmtAvgBPS = reader.ReadInt32();
            int fmtBlockAlign = reader.ReadInt16();
            int bitDepth = reader.ReadInt16();

            if (fmtSize == 18)
            {
                // Read any extra values
                int fmtExtraSize = reader.ReadInt16();
                reader.ReadBytes(fmtExtraSize);
            }

            int dataID = reader.ReadInt32();
            int dataSize = reader.ReadInt32();


            // Store the audio data of the wave file to a byte array. 

            byte[] byteArray = reader.ReadBytes(dataSize);

            int bytesForSamp = bitDepth / 8;
            int samps = fmtSize / bytesForSamp;

            double[] samples = new double[byteArray.Length / 2];
            int i = 0, j = 0;
            while (i < byteArray.Length)
            {
                samples[j++] = BytesToDouble(byteArray[i], byteArray[i + 1]);
                i += 2;
            }

            return samples;
        }

        [Test]
        [TestCase(100, false)]
        [TestCase(250.0, false)]
        [TestCase(440.0, false)]
        [TestCase(1000.0, false)]
        [TestCase(10000.0, false)]
        public void CalculateFFT(double f, bool saveToFile) // f is the signal frequency
        {
            UnmanagedMemoryStream[] streams = new UnmanagedMemoryStream[]
            {
                Resources.Sine100Hz,
                Resources.Sine250Hz,
                Resources.Sine440Hz,
                Resources.Sine1000Hz,
                Resources.Sine10000Hz
            };

            int i = 0;
            switch (f)
            {
                case 100.0:
                    i = 0;
                    break;
                case 250.0:
                    i = 1;
                    break;
                case 440.0:
                    i = 2;
                    break;
                case 1000.0:
                    i = 3;
                    break;
                case 10000.0:
                    i = 4;
                    break;
            }

            UnmanagedMemoryStream stream = streams[i];
            samples = LoadResource(stream);

            Complex[] fft = Fourier.FFT(samples);

            int n = fft.Length;
            double[] magnitude = Fourier.NormalizeMagnitude(fft);
            double[] phase = new double[n];

            double max = magnitude.Max();
            int indexMax = magnitude.ToList().IndexOf(max);

            indexMax.Should().NotBe(0);
            max.Should().Be(
                fft[Fourier.GetFundamentalIndex(fft)].Magnitude * (double)(2m / n)
            );

            double samplingFrequency = 44100.0;
            double scalingFactor = (double)(44100m / n); // 44100: sampling frequency
            double fundamental = indexMax * scalingFactor;
            double threshold = 4.0;
            switch (i)
            {
                case 0:
                    (Fourier.GetFundamental(fft, samplingFrequency) - 98.0).
                        Should().BeGreaterThan(0.0).And.BeLessThan(threshold);
                    break;
                case 1:
                    (Fourier.GetFundamental(fft, samplingFrequency) - 248.0)
                        .Should().BeGreaterThan(0.0).And.BeLessThan(threshold);
                    break;
                case 2:
                    (Fourier.GetFundamental(fft, samplingFrequency) - 438.0)
                        .Should().BeGreaterThan(0.0).And.BeLessThan(threshold);
                    break;
                case 3:
                    (Fourier.GetFundamental(fft, samplingFrequency) - 998.0)
                        .Should().BeGreaterThan(0.0).And.BeLessThan(threshold);
                    break;
                case 4:
                    (Fourier.GetFundamental(fft, samplingFrequency) - 9998.0)
                        .Should().BeGreaterThan(0.0).And.BeLessThan(threshold);
                    break;
            }

            if (saveToFile)
            {
                var csv = new StringBuilder();
                for (int j = 0; j < n; j++)
                {
                    string line = string.Format("{0},{1},{2}",
                        fft[j].Magnitude.ToString(nfi), magnitude[j].ToString(nfi), phase[j].ToString(nfi)
                    );
                    csv.AppendLine(line);
                }

                string path = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    @"fft\" + DateTime.Now.ToString("ffff") + "_" + f + ".csv"
                );
                File.WriteAllText(path, csv.ToString());
            }
        }

        [Test]
        [TestCase(100, 1.0, false)]
        [TestCase(250.0, 1.0, false)]
        [TestCase(440.0, 1.0, false)]
        [TestCase(1000.0, 1.0, false)]
        [TestCase(10000.0, 1.0, false)]
        public void CalculateFFT(double f, double offset, bool saveToFile) // f is the signal frequency
        {
            UnmanagedMemoryStream[] streams = new UnmanagedMemoryStream[]
            {
                Resources.Sine100Hz,
                Resources.Sine250Hz,
                Resources.Sine440Hz,
                Resources.Sine1000Hz,
                Resources.Sine10000Hz
            };

            int i = 0;
            switch (f)
            {
                case 100.0:
                    i = 0;
                    break;
                case 250.0:
                    i = 1;
                    break;
                case 440.0:
                    i = 2;
                    break;
                case 1000.0:
                    i = 3;
                    break;
                case 10000.0:
                    i = 4;
                    break;
            }

            UnmanagedMemoryStream stream = streams[i];
            samples = LoadResourceAndAddOffset(stream, offset);

            Complex[] fft = Fourier.FFT(samples);

            int n = fft.Length;
            double[] magnitude = Fourier.NormalizeMagnitude(fft);
            double[] phase = new double[n];

            double samplingFrequency = 44100.0;
            double scalingFactor = (double)(44100m / n); // 44100: sampling frequency
            double threshold = 4.0;
            switch (i)
            {
                case 0:
                    (Fourier.GetFundamental(fft, samplingFrequency) - 98.0).
                        Should().BeGreaterThan(0.0).And.BeLessThan(threshold);
                    break;
                case 1:
                    (Fourier.GetFundamental(fft, samplingFrequency) - 248.0)
                        .Should().BeGreaterThan(0.0).And.BeLessThan(threshold);
                    break;
                case 2:
                    (Fourier.GetFundamental(fft, samplingFrequency) - 438.0)
                        .Should().BeGreaterThan(0.0).And.BeLessThan(threshold);
                    break;
                case 3:
                    (Fourier.GetFundamental(fft, samplingFrequency) - 998.0)
                        .Should().BeGreaterThan(0.0).And.BeLessThan(threshold);
                    break;
                case 4:
                    (Fourier.GetFundamental(fft, samplingFrequency) - 9998.0)
                        .Should().BeGreaterThan(0.0).And.BeLessThan(threshold);
                    break;
            }

            if (saveToFile)
            {
                var csv = new StringBuilder();
                for (int j = 0; j < n; j++)
                {
                    string line = string.Format("{0},{1},{2}",
                        fft[j].Magnitude.ToString(nfi), magnitude[j].ToString(nfi), phase[j].ToString(nfi)
                    );
                    csv.AppendLine(line);
                }

                string path = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    @"fft\" + DateTime.Now.ToString("ffff") + "_" + f + ".csv"
                );
                File.WriteAllText(path, csv.ToString());
            }
        }

        [Test]
        public void Acquisitions()
        {
            List<double> data = new List<double>();

            var path = @"C:\Users\simod\Desktop\acq\new_acquisitions.csv"; 
            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvParser.ReadLine();

                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    string[] fields = csvParser.ReadFields();
                    data.Add(double.Parse(fields[0], nfi));
                }
            }

            var fft = Fourier.FFT(data.ToArray());

            int n = fft.Length;
            double[] magnitude = Fourier.NormalizeMagnitude(fft);
            double[] phase = new double[n];

            double samplingFrequency = 500;

            double fundamental = Fourier.GetFundamental(fft, samplingFrequency, 50);
        }
    }
}
