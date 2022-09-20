using System;
using System.Linq;

namespace Core.Parameters
{
    /// <summary>
    /// Implement a waveform <see cref="Parameter{T}"/>
    /// </summary>
    public class WaveformParameter : Parameter<double[]>
    {
        private double[] value;

        /// <summary>
        /// The <see cref="WaveformParameter"/> value
        /// </summary>
        public override double[] Value
        {
            get => value;
            set
            {
                if (!value.Equals(this.value))
                {
                    double[] oldValue = this.value;
                    this.value = new double[value.Length];
                    Array.Copy(value, this.value, value.Length);
                    OnValueChanged(new ValueChangedEventArgs(oldValue, this.value));
                }
            }
        }

        /// <summary>
        /// The <see cref="Value"/> as <see cref="double"/> (i.e. the mean value of the array)
        /// </summary>
        public double ValueAsDouble => Value.Average();

        /// <summary>
        /// Create a new instance of <see cref="WaveformParameter"/>
        /// </summary>
        public WaveformParameter() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Create a new instance of <see cref="WaveformParameter"/>
        /// </summary>
        /// <param name="code">The code</param>
        public WaveformParameter(string code) : base(code)
        {
            value = new double[0];
        }

        /// <summary>
        /// Create a new instance of <see cref="WaveformParameter"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="format">The format</param>
        /// <param name="measureUnit">The measure unit</param>
        public WaveformParameter(string code, string measureUnit = "", string format = "") : this(code)
        {
            MeasureUnit = measureUnit;
            Format = format;
        }

        /// <summary>
        /// Create a new instance of <see cref="WaveformParameter"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="value">The initial value</param>
        /// <param name="format">The format</param>
        /// <param name="measureUnit">The measure unit</param>
        public WaveformParameter(string code, double[] value, string measureUnit = "", string format = "")
            : this(code, measureUnit: measureUnit, format: format)
        {
            Value = value;
        }

        /// <summary>
        /// Return a description of the object
        /// See also <see cref="object.ToString()"/>
        /// </summary>
        /// <returns>The description of the object</returns>
        public override string ToString()
        {
            string description = value != null ? $"{value.Average().ToString(Format)}{MeasureUnit}" : double.NaN.ToString();
            return description;
        }
    }
}