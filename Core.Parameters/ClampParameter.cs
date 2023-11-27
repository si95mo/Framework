namespace Core.Parameters
{
    /// <summary>
    /// Implement a numeric parameter with a clamp logic (w/ inclusive limits).
    /// See also <see cref="Parameter{T}"/>, <see cref="NumericParameter"/> and <see cref="IParameter{T}"/>
    /// </summary>
    public class ClampParameter : NumericParameter
    {
        private double value;

        public override double Value
        {
            get => base.Value;
            set
            {
                value = Clamp(value);

                if (!value.Equals(this.value))
                {
                    object oldValue = this.value;
                    this.value = value;
                    OnValueChanged(new ValueChangedEventArgs(oldValue, this.value));
                }
            }
        }

        /// <summary>
        /// The minimum value of the clamp
        /// </summary>
        public NumericParameter Maximum { get; set; }

        /// <summary>
        /// The maximum value of the clamp
        /// </summary>
        public NumericParameter Minimum { get; set; }

        /// <summary>
        /// Create a new instance of <see cref="ClampParameter"/>
        /// </summary>
        /// <remarks>The limits are inclusive</remarks>
        /// <param name="code">The code</param>
        /// <param name="minimum">The minimum value</param>
        /// <param name="maximum">The maximum value</param>
        public ClampParameter(string code, double minimum, double maximum) : base(code)
        {
            Minimum = new NumericParameter($"{code}.{nameof(Minimum)}", value: minimum);
            Maximum = new NumericParameter($"{code}.{nameof(Maximum)}", value: maximum);
        }

        /// <summary>
        /// Create a new instance of <see cref="ClampParameter"/>
        /// </summary>
        /// <remarks>The limits are inclusive</remarks>
        /// <param name="code">The code</param>
        /// <param name="minimum">The minimum value</param>
        /// <param name="maximum">The maximum value</param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        public ClampParameter(string code, double minimum, double maximum, string measureUnit = "", string format = "") : this(code, minimum, maximum)
        {
            MeasureUnit = measureUnit;
            Format = format;

            Minimum.MeasureUnit = measureUnit;
            Minimum.Format = format;
            Maximum.MeasureUnit = measureUnit;
            Maximum.Format = format;
        }

        /// <summary>
        /// Clamp a value inside the defined range
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <returns>The clamped value</returns>
        private double Clamp(double value)
        {
            if (value <= Minimum.Value)
            {
                value = Minimum.Value;
            }
            else if (value >= Maximum.Value)
            {
                value = Maximum.Value;
            }

            return value;
        }

        /// <summary>
        /// Return a description of the object. See also <see cref="object.ToString()"/>
        /// </summary>
        /// <returns>The description of the object</returns>
        public override string ToString()
        {
            string description = $"{Value.ToString(Format)}{MeasureUnit}, in [{Minimum.Value.ToString(Format)}, {Maximum.Value.ToString(Format)}]";

            return description;
        }
    }
}