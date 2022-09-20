using System;

namespace Core.Parameters
{
    /// <summary>
    /// Implement a bool parameter.
    /// See also <see cref="Parameter{T}"/> and <see cref="IParameter{T}"/>
    /// </summary>
    [Serializable]
    public class BoolParameter : Parameter<bool>, IBoolParameter
    {
        /// <summary>
        /// Create a new instance of <see cref="BoolParameter"/>
        /// </summary>
        public BoolParameter() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Create a new instance of <see cref="BoolParameter"/>
        /// </summary>
        /// <param name="code">The code</param>
        public BoolParameter(string code) : base(code)
        { }

        /// <summary>
        /// Create a new instance of <see cref="BoolParameter"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="value">The initial value</param>
        public BoolParameter(string code, bool value) : base(code)
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