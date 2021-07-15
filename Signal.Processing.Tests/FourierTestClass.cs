using FluentAssertions;
using NUnit.Framework;
using Signal.Processing.Tests.Properties;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Signal.Processing.Tests
{
    public class FourierTestClass
    {
        private const int N = 1024;
        private double[] signal;
        private NumberFormatInfo nfi;
        private double[] samples;

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
        [TestCase(100)]
        [TestCase(250.0)]
        [TestCase(440.0)]
        [TestCase(1000.0)]
        [TestCase(10000.0)]
        public void CalculateFFT(double f) // f is the signal frequency
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

            double samplingFrequency = 44100.0;
            (Complex[] fft, double[] magnitude, double[] frequencies, double fundamental, double dcValue) =
                Fourier.FFT(samples, samplingFrequency);
            Console.WriteLine($"\t >> Fundamental frequency: {fundamental:F2}Hz");

            int n = fft.Length;

            double max = magnitude.Max();
            max.Should().NotBe(0);

            double threshold = 0.01;
            (Math.Abs(dcValue) - threshold).Should().BeGreaterThan(-threshold).And.BeLessThan(threshold);

            threshold = 4.0;
            switch (i)
            {
                case 0:
                    (fundamental - 98.0).Should().BeGreaterThan(0.0).And.BeLessThan(threshold);
                    break;

                case 1:
                    (fundamental - 248.0).Should().BeGreaterThan(0.0).And.BeLessThan(threshold);
                    break;

                case 2:
                    (fundamental - 438.0).Should().BeGreaterThan(0.0).And.BeLessThan(threshold);
                    break;

                case 3:
                    (fundamental - 998.0).Should().BeGreaterThan(0.0).And.BeLessThan(threshold);
                    break;

                case 4:
                    (fundamental - 9998.0).Should().BeGreaterThan(0.0).And.BeLessThan(threshold);
                    break;
            }
        }
    }
}