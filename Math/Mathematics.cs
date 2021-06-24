using System;

namespace Mathematics
{
    /// <summary>
    /// Class that contains useful math constants
    /// </summary>
    public class Mathematics
    {
        /// <summary>
        /// Pi Greek 
        /// </summary>
        public double PI => Math.PI;
        /// <summary>
        /// Euler's number
        /// </summary>
        public double E => Math.E;
    }

    /// <summary>
    /// Class that model the numerical set of complex numbers
    /// </summary>
    public class Complex
    {
        private double real;
        private double imaginary;

        /// <summary>
        /// The real part of the <see cref="Complex"/> number
        /// </summary>
        public double Real
        {
            get => real;
            set => real = value;
        }

        /// <summary>
        /// The imaginary part of the <see cref="Complex"/> number
        /// </summary>
        public double Imaginary
        {
            get => imaginary;
            set => imaginary = value;
        }

        /// <summary>
        /// The magnitude of the <see cref="Complex"/> number
        /// </summary>
        public double Magnitude => Math.Sqrt(Math.Pow(real, 2) + Math.Pow(imaginary, 2));

        /// <summary>
        /// The phase of the <see cref="Complex"/> number
        /// </summary>
        public double Phase
        {
            get
            {
                double phase = -90.0; // real == 0 && imaginary <= 0

                if (real != 0)
                    phase = Math.Atan(imaginary / real);
                else
                    if (imaginary > 0)
                    phase = 90;

                return phase;
            }
        }

        /// <summary>
        /// Create a new instance of <see cref="Complex"/>
        /// </summary>
        public Complex()
        {
            real = 0.0;
            imaginary = 0.0;
        }

        /// <summary>
        /// Create a new instance of <see cref="Complex"/>
        /// </summary>
        /// <param name="real">The real part</param>
        /// <param name="imaginary">The imaginary part</param>
        public Complex(double real, double imaginary)
        {
            this.real = real;
            this.imaginary = imaginary;
        }

        public override string ToString()
        {
            string descrition = $"{real}, {imaginary}i";
            return descrition;
        }

        /// <summary>
        /// Convert a number from polar coordinates to rectangular ones.
        /// </summary>
        /// <param name="r">The magnitude</param>
        /// <param name="phi">The phase</param>
        /// <returns>The <see cref="Complex"/> number</returns>
        public static Complex PolarToRectangular(double r, double phi)
        {
            Complex number = new Complex(r * Math.Cos(phi), r * Math.Sin(phi));
            return number;
        }

        /// <summary>
        /// Sum two <see cref="Complex"/> numbers
        /// </summary>
        /// <param name="x">The first number</param>
        /// <param name="y">The second number</param>
        /// <returns>The result of <paramref name="x"/> + <paramref name="y"/></returns>
        public static Complex operator +(Complex x, Complex y)
        {
            Complex sum = new Complex((x.Real + y.Real), (x.Imaginary + y.Imaginary));
            return sum;
        }

        /// <summary>
        /// Subtract two <see cref="Complex"/> numbers
        /// </summary>
        /// <param name="x">The first number</param>
        /// <param name="y">The second number</param>
        /// <returns>The result of <paramref name="x"/> - <paramref name="y"/></returns>
        public static Complex operator -(Complex x, Complex y)
        {
            Complex diff = new Complex((x.Real - y.Real), (x.Imaginary - y.Imaginary));
            return diff;
        }

        /// <summary>
        /// Multiply two <see cref="Complex"/> numbers
        /// </summary>
        /// <param name="x">The first number</param>
        /// <param name="y">The second number</param>
        /// <returns>The result of <paramref name="x"/> * <paramref name="y"/></returns>
        public static Complex operator *(Complex x, Complex y)
        {
            Complex mul = new Complex(
                (x.Real * y.Real) - (x.Imaginary * y.Imaginary),
                (x.Real * y.Imaginary) + (x.Imaginary * y.Real)
            );
            return mul;
        }

        /// <summary>
        /// Multiply a <see cref="Complex"/> number by a real number
        /// </summary>
        /// <param name="x">The <see cref="Complex"/> number</param>
        /// <param name="y">The real number</param>
        /// <returns>The result of <paramref name="x"/> * <paramref name="y"/>, 
        /// where <paramref name="y"/> is <see cref="double"</returns>
        public static Complex operator *(Complex x, double y)
            => x * new Complex(y, 0);
    }
}
