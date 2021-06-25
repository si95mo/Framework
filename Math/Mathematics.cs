using System;
using System.Numerics;

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

        /// <summary>
        /// The imaginary unit
        /// </summary>
        public Complex I => Complex.ImaginaryOne;

        /// <summary>
        /// Calculate the magnitude of each element 
        /// in an array of <see cref="Complex"/>.
        /// See <see cref="Complex.Magnitude"/>
        /// </summary>
        /// <param name="data">The data of which calculate the magnitude</param>
        /// <param name="normalized">Specifies whether the magnitude has to 
        /// be normalized (<see langword="true"/>) or not (<see langword="false"/>)</param>
        /// <returns>The array of magnitudes</returns>
        public static double[] Magnitudes(Complex[] data, bool normalized = false)
        {
            int n = data.Length;
            double[] magnitude = new double[n];

            if (!normalized)
                for (int i = 0; i < n; i++)
                    magnitude[i] = data[i].Magnitude;
            else
                for (int i = 0; i < n; i++)
                    magnitude[i] = data[i].Magnitude / n;

            return magnitude;
        }

        /// <summary>
        /// Calculate the phase of each element 
        /// in an array of <see cref="Complex"/>.
        /// See <see cref="Complex.Phase"/>
        /// </summary>
        /// <param name="data">The data of which calculate the phase</param>
        /// <returns>The array of phases</returns>
        public static double[] Phases(Complex[] data)
        {
            int n = data.Length;
            double[] phase = new double[n];

            for (int i = 0; i < n; i++)
                phase[i] = data[i].Phase;

            return phase;
        }
    }
}
