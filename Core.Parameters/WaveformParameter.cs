using System;
using System.Linq;

namespace Core.Parameters
{
    /// <summary>
    /// Implement a waveform <see cref="Parameter{T}"/>
    /// </summary>
    public class WaveformParameter : Parameter<double[]>
    {
        /// <summary>
        /// The <see cref="WaveformParameter"/> value;
        /// </summary>
        public override double[] Value => value;

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
        { }

        /// <summary>
        /// Create a new instance of <see cref="WaveformParameter"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="format">The format</param>
        /// <param name="measureUnit">The measure unit</param>
        public WaveformParameter(string code, string measureUnit = "", string format = "") : this(code)
        {
            this.measureUnit = measureUnit;
            this.format = format;
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
        /// The <see cref="WaveformParameter"/> measure unit
        /// </summary>
        public new string MeasureUnit
        {
            get => measureUnit;
            set => measureUnit = value;
        }

        /// <summary>
        /// The <see cref="WaveformParameter"/> format
        /// </summary>
        public new string Format
        {
            get => format;
            set => format = value;
        }

        /// <summary>
        /// Return a description of the object
        /// See also <see cref="object.ToString()"/>
        /// </summary>
        /// <returns>The description of the object</returns>
        public override string ToString()
        {
            string description = double.NaN.ToString();

            if (value != null)
                description = $"{value.Average().ToString(format)}{measureUnit}";

            return description;
        }
    }
}
