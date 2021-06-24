using Mathematics;
using System;
using System.Linq;

namespace Signal.Processing
{
    /// <summary>
    /// Class that calculate the Fourier transform.
    /// See <see href="https://www.egr.msu.edu/classes/ece480/capstone/fall11/group06/style/Application_Note_ChrisOakley.pdf"/>
    /// </summary>
    public class Fourier
    {
        /// <summary>
        /// Discrete Fourier Transform (DFT)
        /// </summary>
        /// <param name="x">The array of <see cref="Complex"/> number to transform</param>
        /// <returns>The result of the transformation</returns>
        public static Complex[] DFT(Complex[] x)
        {
            int n = x.Length;
            Complex[] X = new Complex[n];

            for (int k = 0; k < n; k++)
            {
                X[k] = new Complex(0, 0);

                for (int h = 0; h < n; h++)
                {
                    Complex tmp = Complex.PolarToRectangular(1, -2 * Math.PI * h * k / n);
                    tmp *= x[h];
                    X[k] += tmp;
                }
            }

            return X;
        }

        /// <summary>
        /// Discrete Fourier Transform (DFT)
        /// </summary>
        /// <param name="x">The array of <see cref="double"/> number to transform</param>
        /// <returns>The result of the transformation</returns>
        public static Complex[] DFT(double[] x)
        {
            Complex[] xc = new Complex[x.Length];

            for (int i = 0; i < x.Length; i++)
            {
                xc[i] = new Complex(x[i], 0);
            }

            return DFT(xc);
        }

        /// <summary>
        /// Fast Fourier Transform (FFT)
        /// </summary>
        /// <param name="x">The array of <see cref="Complex"/> number to transform</param>
        /// <returns>The result of the transformation</returns>
        public static Complex[] FFT(Complex[] x)
        {
            if (!IsPowerOfTwo(x.Length))
                x = DoZeroPadding(x);

            int n = x.Length;
            Complex[] X = new Complex[n];

            Complex[] d, D, e, E;

            // Base case
            if (n == 1)
            {
                X[0] = x[0];
                return X;
            }

            int k;

            e = new Complex[n / 2];
            d = new Complex[n / 2];

            for (k = 0; k < n / 2; k++)
            {
                e[k] = x[2 * k];
                d[k] = x[2 * k + 1];
            }

            D = FFT(d);
            E = FFT(e);

            for (k = 0; k < n / 2; k++)
            {
                Complex tmp = Complex.PolarToRectangular(1, -2 * Math.PI * k / n);
                D[k] *= tmp;
            }

            for (k = 0; k < n / 2; k++)
            {
                X[k] = E[k] + D[k];
                X[k + n / 2] = E[k] - D[k];
            }

            return X;
        }

        /// <summary>
        /// Discrete Fourier Transform (FFT)
        /// </summary>
        /// <param name="x">The array of <see cref="double"/> number to transform</param>
        /// <returns>The result of the transformation</returns>
        public static Complex[] FFT(double[] x)
        {
            Complex[] xc = new Complex[x.Length];

            for (int i = 0; i < x.Length; i++)
            {
                xc[i] = new Complex(x[i], 0);
            }

            return FFT(xc);
        }

        /// <summary>
        /// Determines whether the input is a power of 2
        /// </summary>
        /// <param name="n">The number to test</param>
        /// <returns><see langword="true"/> if the input is a power of 2,
        /// <see langword="false"/> otherwise</returns>
        private static bool IsPowerOfTwo(int n)
        {
            bool isPower = n > 0 && (n & (n - 1)) == 0;
            return isPower;
        }

        /// <summary>
        /// Perform a zero-padding operation
        /// </summary>
        /// <param name="x">The array to fill with zeros</param>
        /// <returns>The padded array</returns>
        private static Complex[] DoZeroPadding(Complex[] x)
        {
            int size = (int)Math.Round(
                Math.Pow(
                    2,
                    Math.Ceiling(
                        Math.Log(
                            x.Length,
                            2
                        )
                    )
                )
            );
            Complex[] xPadded = new Complex[size];

            for (int i = 0; i < size; i++)
            {
                if (i < x.Length)
                    xPadded[i] = x[i];
                else
                    xPadded[i] = new Complex(0, 0);
            }

            return xPadded;
        }

        /// <summary>
        /// Perform a magnitude normalization of the output of 
        /// a <see cref="FFT(Complex[])"/>
        /// </summary>
        /// <param name="fft">The transformed array</param>
        /// <returns>The normalized magnitude array</returns>
        public static double[] NormalizeMagnitude(Complex[] fft)
        {
            int n = fft.Length;
            double[] magnitudeNormalized = new double[n];

            double scalingFactor = (double)(n);
            double tmp, magnitude;
            for (int i = 0; i < n; i++)
            {
                magnitude = fft[i].Magnitude;
                tmp = magnitude * 1 / scalingFactor;
                magnitudeNormalized[i] = tmp;
            }

            return magnitudeNormalized;
        }

        /// <summary>
        /// Calculate the corresponding frequency for each
        /// element of the output of an <see cref="FFT(Complex[])"/>
        /// </summary>
        /// <param name="fft">The transformed array</param>
        /// <param name="samplingFrequency">The sampling frequency (in Hz)</param>
        /// <returns>The array with all the frequencies</returns>
        public static double[] GetFrequencies(Complex[] fft, double samplingFrequency)
        {
            int n = fft.Length;
            double[] frequency = new double[n];
            double scalingFactor = samplingFrequency / n;

            for (int i = 0; i < n; i++)
                frequency[i] = i * scalingFactor;

            return frequency;
        }

        /// <summary>
        /// Retrieve the index relative to the fundamental frequency
        /// in the output of an <see cref="FFT(Complex[])"/>
        /// </summary>
        /// <param name="fft">The transformed array</param>
        /// <returns>The fundamental index</returns>
        public static int GetFundamentalIndex(Complex[] fft)
        {
            int n = fft.Length;

            if (n == 0) // base case
                return 0;

            int frequencyIndex;

            double[] magnitude = NormalizeMagnitude(fft);

            double max = magnitude.Max();
            frequencyIndex = magnitude.ToList().IndexOf(max);

            if (frequencyIndex == 0)
            {
                var tmp = new Complex[n - 1];
                Array.Copy(fft, 1, tmp, 0, n - 1);

                frequencyIndex = GetFundamentalIndex(tmp);
            }

            return frequencyIndex;
        }

        /// <summary>
        /// Calculate the fundamental frequency of a transformed signal
        /// </summary>
        /// <param name="fft">The transformed array</param>
        /// <param name="samplingFrequency">The sampling frequency (in Hz)</param>
        /// <returns>The fundamental frequency (in Hz)</returns>
        public static double GetFundamental(Complex[] fft, double samplingFrequency)
        {
            int n = fft.Length;
            double frequency;

            int fundamentalIndex = GetFundamentalIndex(fft);
            double scalingFactor = samplingFrequency / n;

            frequency = fundamentalIndex * scalingFactor;

            return frequency;
        }

        /// <summary>
        /// Calculate the fundamental frequency of a transformed signal
        /// acquired via oversampling
        /// </summary>
        /// <param name="fft">The transformed array</param>
        /// <param name="samplingFrequency">The sampling frequency (in Hz)</param>
        /// <param name="samplingFactor">The sampling factor (N)</param>
        /// <returns>The fundamental frequency (in Hz)</returns>
        public static double GetFundamental(Complex[] fft, double samplingFrequency, double samplingFactor)
        {
            int n = fft.Length;
            double frequency;

            int fundamentalIndex = GetFundamentalIndex(fft);
            // d = 2M * f / N
            double scalingFactor = 2d * samplingFactor * samplingFrequency / n;

            frequency = fundamentalIndex * scalingFactor;

            return frequency;
        }
    }
}
