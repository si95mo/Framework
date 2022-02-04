using Core;
using Hardware;
using System;
using System.Collections.Generic;

namespace Signal.Processing
{
    /// <summary>
    /// Implement Kalman filtering
    /// </summary>
    public class Kalman : IProperty
    {
        private string code;

        private double a, h, q, r, p;
        private AnalogOutput x;
        private List<double> filtered;

        /// <summary>
        /// The filtered signal (using <see cref="Kalman"/>)
        /// </summary>
        public List<double> Filtered => filtered;

        /// <summary>
        /// The <see cref="Kalman"/> filter output
        /// </summary>
        public AnalogOutput X => x;

        public string Code => code;

        public object ValueAsObject
        {
            get => x.Value;
            set => x.Value = (double)value;
        }

        public Type Type => typeof(Kalman);

        /// <summary>
        /// Create a new instance of <see cref="Kalman"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="a">The process state matrix (scalar in this case)</param>
        /// <param name="h">The process output matrix (scalar in this case</param>
        /// <param name="q">The covariance of the process noise</param>
        /// <param name="r">The covariance of the observation noise</param>
        /// <param name="p">The error covariance</param>
        /// <param name="x">The initial input</param>
        public Kalman(string code, double a, double h, double q, double r,
            double p, Channel<double> x)
        {
            this.code = code;
            this.a = a;
            this.h = h;
            this.q = q;
            this.r = r;
            this.p = p;

            this.x = new AnalogOutput("X", x.MeasureUnit, x.Format);
            filtered = new List<double>();

            x.ValueChanged += Input_ValueChanged;
        }

        private void Input_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            double filteredData = Filter((double)e.NewValue, (double)e.OldValue);
            filtered.Add(filteredData);
        }

        /// <summary>
        /// Filter the input
        /// </summary>
        /// <param name="newInput">The new input value</param>
        /// <param name="oldInput">The old input value</param>
        /// <returns>The filtered output</returns>
        private double Filter(double newInput, double oldInput)
        {
            // Time update - prediction
            x.Value = a * oldInput;
            p = a * p * a + q;

            // Measurement update - correction
            double k = p * h / (h * p * h + r);
            x.Value += k * (newInput - h * x.Value);
            p = (1 - k * h) * p;

            return x.Value;
        }
    }
}