using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Parameters
{
    /// <summary>
    /// Implement a string parameter.
    /// See also <see cref="Parameter{T}"/> and <see cref="IParameter{T}"/>
    /// </summary>
    public class StringParameter : Parameter<string>, IStringParameter
    {
        /// <summary>
        /// The <see cref="StringParameter"/> value;
        /// </summary>
        public override string Value => value;

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
