﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// Implement a bool parameter.
    /// See also <see cref="Parameter{T}"/> and <see cref="IParameter{T}"/>
    /// </summary>
    public class BooleanParameter : Parameter<bool>, IBooleanParameter
    {
        /// <summary>
        /// The <see cref="BooleanParameter"/> value;
        /// </summary>
        public override bool Value => value;

        /// <summary>
        /// Create a new instance of <see cref="BooleanParameter"/>
        /// </summary>
        public BooleanParameter() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Create a new instance of <see cref="BooleanParameter"/>
        /// </summary>
        /// <param name="code">The code</param>
        public BooleanParameter(string code) : base(code)
        { }

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
