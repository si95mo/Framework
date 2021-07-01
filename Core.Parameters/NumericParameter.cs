using System;

namespace Core.Parameters
{
    /// <summary>
    /// Implement a numeric parameter.
    /// See also <see cref="Parameter{T}"/> and <see cref="IParameter{T}"/>
    /// </summary>
    public class NumericParameter : Parameter<double>, INumericParameter
    {
        /// <summary>
        /// The <see cref="NumericParameter"/> value;
        /// </summary>
        public override double Value => value;

        /// <summary>
        /// Create a new instance of <see cref="NumericParameter"/>
        /// </summary>
        public NumericParameter() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Create a new instance of <see cref="NumericParameter"/>
        /// </summary>
        /// <param name="code">The code</param>
        public NumericParameter(string code) : base(code)
        { }

        /// <summary>
        /// Create a new instance of <see cref="NumericParameter"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="format">The format</param>
        /// <param name="measureUnit">The measure unit</param>
        public NumericParameter(string code, string measureUnit = "", string format = "") : this(code)
        {
            this.measureUnit = measureUnit;
            this.format = format;
        }

        /// <summary>
        /// The <see cref="NumericParameter"/> measure unit
        /// </summary>
        public new string MeasureUnit
        {
            get => measureUnit;
            set => measureUnit = value;
        }

        /// <summary>
        /// The <see cref="NumericParameter"/> format
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
            string description = $"{value.ToString(format)}{measureUnit}";

            return description;
        }
    }
}