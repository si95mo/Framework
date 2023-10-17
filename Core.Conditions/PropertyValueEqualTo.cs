namespace Core.Conditions
{
    /// <summary>
    /// Implement an <see cref="ICondition"/> that will be <see langword="true"/>
    /// when a property value is equal to the one passed
    /// </summary>
    public class PropertyValueEqualTo<T> : FlyweightCondition
    {
        private T valueToTest;

        /// <summary>
        /// Create a new instance of <see cref="PropertyValueEqualTo{T}"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="property">The <see cref="IProperty{T}"/> of which test the value</param>
        /// <param name="valueToTest">The target value</param>
        public PropertyValueEqualTo(string code, IProperty<T> property, T valueToTest) : base(code)
        {
            this.valueToTest = valueToTest;
            property.ValueChanged += Property_ValueChanged;
        }

        /// <summary>
        /// Create a new instance of <see cref="PropertyValueEqualTo{T}"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="property">The <see cref="IProperty{T}"/> of which test the value</param>
        /// <param name="propertyToTest">The target <see cref="IProperty{T}"/></param>
        public PropertyValueEqualTo(string code, IProperty<T> property, IProperty<T> propertyToTest) : base(code)
        {
            valueToTest = propertyToTest.Value;

            propertyToTest.ValueChanged += (s, e) => valueToTest = propertyToTest.Value;
            property.ValueChanged += Property_ValueChanged;
        }

        private void Property_ValueChanged(object sender, ValueChangedEventArgs e)
            => Value = e.NewValue.Equals(valueToTest);
    }
}