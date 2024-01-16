using Core.Parameters.IReadOnly;
using System;

namespace Core.Parameters
{
    /// <summary>
    /// Implement a numeric parameter.
    /// See also <see cref="Parameter{T}"/> and <see cref="IParameter{T}"/>
    /// </summary>
    [Serializable]
    public class NumericParameter : Parameter<double>, INumericParameter, IReadOnlyNumericParameter
    {
        public int ValueAsInt => (int)Value;

        public float ValueAsFloat => (float)Value;

        public byte ValueAsByte => (byte)Value;

        public byte[] ValueAsByteArray => BitConverter.GetBytes(Value);

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
            MeasureUnit = measureUnit;
            Format = format;
        }

        /// <summary>
        /// Create a new instance of <see cref="NumericParameter"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="value">The initial value</param>
        /// <param name="format">The format</param>
        /// <param name="measureUnit">The measure unit</param>
        public NumericParameter(string code, double value, string measureUnit = "", string format = "")
            : this(code, measureUnit: measureUnit, format: format)
        {
            Value = value;
        }

        /// <summary>
        /// Return a description of the object. See also <see cref="object.ToString()"/>
        /// </summary>
        /// <returns>The description of the object</returns>
        public override string ToString()
        {
            string description = $"{Value.ToString(Format)}{MeasureUnit}";

            return description;
        }
    }
}