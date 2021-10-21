namespace Signal.Processing
{
    /// <summary>
    /// Implement Kalman filtering
    /// </summary>
    public class Kalman
    {
        private double a, h, q, r, p, x;

        /// <summary>
        /// Create a new instance of <see cref="Kalman"/>
        /// </summary>
        /// <param name="a">Kalman coefficient</param>
        /// <param name="h">Kalman coefficient</param>
        /// <param name="q">Kalman coefficient</param>
        /// <param name="r">Kalman coefficient</param>
        /// <param name="p">Kalman coefficient</param>
        /// <param name="x">Initial input</param>
        public Kalman(double a, double h, double q, double r, double p, double x)
        {
            this.a = a;
            this.h = h;
            this.q = q;
            this.r = r;
            this.p = p;
            this.x = x;
        }

        /// <summary>
        /// Filter the input
        /// </summary>
        /// <param name="input">The input</param>
        /// <returns>The filtered output</returns>
        public double Filter(double input)
        {
            // Time update - prediction
            x = a * x;
            p = a * p * a + q;

            // Measurement update - correction
            double k = p * h / (h * p * h + r);
            x = x + k * (input - h * x);
            p = (1 - k * h) * p;

            return x;
        }
    }
}