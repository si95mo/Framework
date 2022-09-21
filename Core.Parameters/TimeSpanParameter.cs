using System;

namespace Core.Parameters
{
    /// <summary>
    /// Implement a time span parameter.
    /// See also <see cref="Parameter{T}"/> and <see cref="IParameter{T}"/>
    /// </summary>
    [Serializable]
    public class TimeSpanParameter : Parameter<TimeSpan>, ITimeSpanParameter
    {
        /// <summary>
        /// Represent the current <see cref="TimeSpan"/> stored in
        /// <see cref="Parameter{T}.Value"/> as seconds
        /// </summary>
        public double ValueAsSeconds
        {
            get => Value.TotalSeconds;
            set => Value = TimeSpan.FromSeconds(value);
        }

        /// <summary>
        /// Represent the current <see cref="TimeSpan"/> stored in
        /// <see cref="Parameter{T}.Value"/> as milliseconds
        /// </summary>
        public double ValueAsMilliseconds
        {
            get => Value.TotalMilliseconds;
            set => Value = TimeSpan.FromMilliseconds(value);
        }

        /// <summary>
        /// Create a new instance of <see cref="TimeSpanParameter"/>
        /// </summary>
        public TimeSpanParameter() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Create a new instance of <see cref="TimeSpanParameter"/>
        /// </summary>
        /// <param name="code">The code</param>
        public TimeSpanParameter(string code) : base(code)
        { }

        /// <summary>
        /// Create a new instance of <see cref="TimeSpanParameter"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="value">The initial value</param>
        public TimeSpanParameter(string code, TimeSpan value) : base(code)
            => Value = value;

        /// <summary>
        /// Create a new instance of <see cref="TimeSpanParameter"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="value">The initial value (in milliseconds)</param>
        public TimeSpanParameter(string code, double value) : this(code, TimeSpan.FromMilliseconds(value))
        { }

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