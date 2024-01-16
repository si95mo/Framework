using Core.Parameters.IReadOnly;
using System;

namespace Core.Parameters
{
    /// <summary>
    /// Implement an enum parameter.
    /// See also <see cref="Parameter{T}"/>, <see cref="IParameter"/>
    /// and <see cref="IEnumParameter{T}"/>
    /// </summary>
    /// <typeparam name="T">The <see cref="Enum"/> type of the wrapped parameter</typeparam>
    [Serializable]
    public class EnumParameter<T> : Parameter<T>, IEnumParameter<T>, IReadOnlyEnumParameter<T> where T : Enum
    {
        /// <summary>
        /// The <see cref="EnumParameter{T}.Value"/> value as <see cref="int"/>
        /// </summary>
        public int ValueAsInt => Convert.ToInt32(Value);

        /// <summary>
        /// Create a new instance of <see cref="StringParameter"/>
        /// </summary>
        public EnumParameter() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Create a new instance of <see cref="StringParameter"/>
        /// </summary>
        /// <param name="code">The code</param>
        public EnumParameter(string code) : base(code)
        {
            Value = default;
        }

        /// <summary>
        /// Create a new instance of <see cref="StringParameter"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="value">The initial value</param>
        public EnumParameter(string code, T value) : base(code)
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
            string description = $"{Value}";

            return description;
        }
    }
}