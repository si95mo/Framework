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
            get => value.TotalSeconds;
            set => this.value = TimeSpan.FromSeconds(value);
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