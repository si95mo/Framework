namespace Core.Conditions
{
    /// <summary>
    /// Implement an <see cref="ICondition"/> that will be <see langword="true"/> when the value of a <see cref="IProperty"/> change
    /// </summary>
    public class PropertyValueChanged : FlyweightCondition
    {
        private bool value;

        /// <summary>
        /// The value of the <see cref="PropertyValueChanged"/>
        /// </summary>
        /// <remarks>
        /// <see cref="Value"/> will be <see langword="true"/> when the <see cref="IProperty"/> associated with the <see cref="PropertyValueChanged"/>
        /// change its value and will automatically become <see langword="false"/> once it will be accessed via the <see cref="Value"/> property getter!
        /// </remarks>
        public override bool Value
        {
            get
            {
                bool actualValue = value;
                value = false;

                return actualValue;
            }
            set
            {
                if (!value.Equals(this.value))
                {
                    object oldValue = this.value;
                    this.value = value;
                    OnValueChanged(new ValueChangedEventArgs(oldValue, this.value));

                    this.value = false;
                }
            }
        }

        /// <summary>
        /// Create a new instance <see cref="PropertyValueChanged"/>
        /// with default attribute values
        /// </summary>
        /// <param name="code">The code</param>
        private PropertyValueChanged(string code) : base(code)
        {
            Value = false;
        }

        /// <summary>
        /// Create a new instance of <see cref="PropertyValueChanged"/> for
        /// an <see cref="IProperty"/> with a <see cref="bool"/> value
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="property">The <see cref="IProperty"/></param>
        public PropertyValueChanged(string code, IProperty<bool> property) : this(code)
        {
            property.ValueChanged += Property_ValueChanged;
        }

        /// <summary>
        /// Create a new instance of <see cref="PropertyValueChanged"/>for
        /// an <see cref="IProperty"/> with a <see cref="double"/> value
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="property">The <see cref="IProperty"/></param>
        public PropertyValueChanged(string code, IProperty<double> property) : this(code)
        {
            property.ValueChanged += Property_ValueChanged;
        }

        /// <summary>
        /// Create a new instance of <see cref="PropertyValueChanged"/>for
        /// an <see cref="IProperty"/> with a <see cref="string"/> value
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="property">The <see cref="IProperty"/></param>
        public PropertyValueChanged(string code, IProperty<string> property) : this(code)
        {
            property.ValueChanged += Property_ValueChanged;
        }

        private void Property_ValueChanged(object sender, ValueChangedEventArgs e)
            => Value = true;
    }
}