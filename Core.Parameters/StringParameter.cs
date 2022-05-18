using System;

namespace Core.Parameters
{
    /// <summary>
    /// Implement a string parameter.
    /// See also <see cref="Parameter{T}"/> and <see cref="IParameter{T}"/>
    /// </summary>
    [Serializable]
    public class StringParameter : Parameter<string>, IStringParameter
    {
        /// <summary>
        /// The <see cref="StringParameter"/> value;
        /// </summary>
        public override string Value => value;

        /// <summary>
        /// The <see cref="Value"/> as <see cref="double"/>
        /// </summary>
        /// <remarks>If the conversion is not possible, then 0 is returned!</remarks>
        public double ValueAsDouble
        {
            get
            {
                double.TryParse(Value, out double valueAsDouble);
                return valueAsDouble;
            }
        }

        /// <summary>
        /// The <see cref="Value"/> as <see cref="float"/>
        /// </summary>
        /// <remarks>If the conversion is not possible, then 0 is returned!</remarks>
        public float ValueAsFloat
        {
            get
            {
                float.TryParse(Value, out float valueAsFloat);
                return valueAsFloat;
            }
        }

        /// <summary>
        /// The <see cref="Value"/> as <see cref="int"/>
        /// </summary>
        /// <remarks>If the conversion is not possible, then 0 is returned!</remarks>
        public int ValueAsInt
        {
            get
            {
                int.TryParse(Value, out int valueAsInt);
                return valueAsInt;
            }
        }

        /// <summary>
        /// The <see cref="Value"/> as <see cref="bool"/>
        /// </summary>
        /// <remarks>If the conversion is not possible, then <see langword="false"/> is returned!</remarks>
        public bool ValueAsBool
        {
            get
            {
                bool.TryParse(Value, out bool valueAsBool);
                return valueAsBool;
            }
        }

        /// <summary>
        /// Create a new instance of <see cref="StringParameter"/>
        /// </summary>
        public StringParameter() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Create a new instance of <see cref="StringParameter"/>
        /// </summary>
        /// <param name="code">The code</param>
        public StringParameter(string code) : base(code)
        {
            value = "";
        }

        /// <summary>
        /// Create a new instance of <see cref="StringParameter"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="value">The initial value</param>
        public StringParameter(string code, string value) : this(code)
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
            string description = $"{value}";

            return description;
        }
    }
}