using System;

namespace Core.Parameters
{
    public class EnumParameter<T> : Parameter<T>, IEnumParameter<T> where T : Enum
    {
        /// <summary>
        /// The <see cref="EnumParameter{T}"/> value
        /// </summary>
        public override T Value => value;

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
            value = default(T);
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
