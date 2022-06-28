using FFTWSharp;
using System;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Signal.Processing
{
    /// <summary>
    /// Represent the result of <see cref="Fourier.FFT(double[], double)"/>
    /// </summary>
    public struct FourierResult
    {
        /// <summary>
        /// Create a new instance of <see cref="FourierResult"/>
        /// </summary>
        /// <param name="fft">The FFT array</param>
        /// <param name="magnitudes">The magnitudes (in dB)</param>
        /// <param name="frequencies">The frequencies (in Hz)</param>
        /// <param name="fundamental">The fundamental frequency (in Hz)</param>
        /// <param name="dcValue">The DC value (in dB)</param>
        public FourierResult(Complex[] fft, double[] magnitudes, double[] frequencies, double fundamental, double dcValue)
        {
            FFT = fft;
            Magnitudes = magnitudes;
            Frequencies = frequencies;
            FundamentalFrequency = fundamental;
            DcValue = dcValue;
        }

        /// <summary>
        /// The FFT array
        /// </summary>
        public Complex[] FFT { get; }

        /// <summary>
        /// The magnitudes array (in dB)
        /// </summary>
        public double[] Magnitudes { get; }

        /// <summary>
        /// The frequencies array (in Hz)
        /// </summary>
        public double[] Frequencies { get; }

        /// <summary>
        /// The fundamental frequency (in Hz)
        /// </summary>
        public double FundamentalFrequency { get; }

        /// <summary>
        /// The DC value (in dB)
        /// </summary>
        public double DcValue { get; }
    }

    /// <summary>
    /// Class that calculate the Fourier transform. <br/>
    /// To perform a "real-time" FFT, consider using an <see cref="Hardware.MultiSampleAnalogInput"/>
    /// and calculate the transform in the ValueChanged event
    /// </summary>
    public class Fourier
    {
        /// <summary>
        /// Compute the Fast Fourier Transform.
        /// See also <see cref="FFT(double[], double)"/>
        /// </summary>
        /// <remarks>
        /// To perform a "real-time" FFT, consider using an <see cref="Hardware.MultiSampleAnalogInput"/>
        /// and calculate the transform in the ValueChanged event
        /// </remarks>
        /// <param name="data">The data to transform</param>
        /// <returns>The data transformed in the frequency domain</returns>
        public static Complex[] FFT(double[] data)
        {
            FourierResult result = FFT(data, 0);

            return result.FFT;
        }

        /// <summary>
        /// Compute the Fast Fourier Transform.
        /// </summary>
        /// <param name="data">The data to transform</param>
        /// <param name="samplingFrequency">The sampling frequency in Hz</param>
        /// <returns>The data in the frequency domain, the array with the magnitude (dB)
        /// and frequencies (Hz), the fundamental frequency and the DC value, in this order</returns>
        public static FourierResult FFT(double[] data, double samplingFrequency)
        {
            Complex[] fft = ToNumericComplex(CalculateFFT(data));

            double[] magnitude = Mathematics.Mathematics.CalculateMagnitudes(fft, true);
            double max = magnitude.Max();
            double indexOfMax = magnitude.ToList().IndexOf(max);

            double scalingFactor = samplingFrequency / (2 * fft.Length);
            double fundamentalFrequency = indexOfMax * scalingFactor;

            double[] frequencies = new double[magnitude.Length];
            for (int i = 0; i < frequencies.Length; i++)
                frequencies[i] = i * scalingFactor;

            double dcValue = magnitude[0];

            FourierResult result = new FourierResult(fft, magnitude, frequencies, fundamentalFrequency, dcValue);
            return result;
        }

        /// <summary>
        /// Compute the FFT of an array of real data
        /// </summary>
        /// <param name="data">The data</param>
        /// <returns>The transformed data in the frequency domain</returns>
        private static double[] CalculateFFT(double[] data)
        {
            // Convert real data to complex
            data = ToComplex(data);
            int n = data.Length;

            // Allocate memory for input and output
            IntPtr ptr = fftw.malloc(n * sizeof(double));
            Marshal.Copy(data, 0, ptr, n);

            // Plan FFT and execute it
            IntPtr plan = fftw.dft_1d(n / 2, ptr, ptr, fftw_direction.Forward, fftw_flags.Estimate);
            fftw.execute(plan);

            // Output
            double[] fft = new double[n / 2];
            Marshal.Copy(ptr, fft, 0, n / 2);

            // Clean-up
            fftw.destroy_plan(plan);
            fftw.free(ptr);
            fftw.cleanup();

            return fft;
        }

        /// <summary>
        /// Compute the IFFT  of the data
        /// </summary>
        /// <param name="data">The data to transform</param>
        /// <returns>The transformed in the time domain</returns>
        public static double[] IFFT(double[] data)
        {
            int n = data.Length;

            // Allocate memory for input and output
            IntPtr ptr = fftw.malloc(n * sizeof(double));
            Marshal.Copy(data, 0, ptr, n);

            // Plan IFFT and execute it
            IntPtr plan = fftw.dft_1d(n / 2, ptr, ptr, fftw_direction.Backward, fftw_flags.Estimate);
            fftw.execute(plan);

            // Output
            double[] ifft = new double[n];
            Marshal.Copy(ptr, ifft, 0, n);

            // Clean-up
            fftw.destroy_plan(plan);
            fftw.free(ptr);
            fftw.cleanup();

            // Scale the output
            for (int i = 0, nh = n / 2; i < n; i++)
                ifft[i] /= nh;

            return ifft;
        }

        /// <summary>
        /// Transform and array of real numbers in an
        /// array of complex data (with respect to FFTW
        /// representation standard of complex numbers).
        /// </summary>
        /// <param name="real">The array to convert</param>
        /// <returns>The converted array</returns>
		private static double[] ToComplex(double[] real)
        {
            int n = real.Length;
            double[] complex = new double[2 * n];

            for (int i = 0; i < n; i++)
                complex[2 * i] = real[i];

            return complex;
        }

        /// <summary>
        /// Transform and array of complex data (FFTW representation)
        /// in an array of <see cref="Complex"/>.
        /// </summary>
        /// <param name="data">The array to convert</param>
        /// <returns>The converted array</returns>
        private static Complex[] ToNumericComplex(double[] data)
        {
            int n = data.Length;
            Complex[] complex = new Complex[n / 2];

            for (int i = 0; i < n / 2; i++)
                complex[i] = new Complex(data[2 * i], data[2 * i + 1]);

            return complex;
        }
    }
}