namespace Core.Conditions
{
    /// <summary>
    /// Implement an <see cref="ICondition"/> that will be <see langword="true"/>
    /// when a property value is equal to the one passed
    /// </summary>
    public class PropertyValueEqualTo : ConditionFlyweight
    {
        private double valueToTest;

        /// <summary>
        /// Create a new instance of <see cref="PropertyValueEqualTo"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="property">The <see cref="IProperty{T}"/> of which test the value</param>
        /// <param name="valueToTest">The target value</param>
        public PropertyValueEqualTo(string code, IProperty<double> property, double valueToTest) : base(code)
        {
            this.valueToTest = valueToTest;

            property.ValueChanged += Property_ValueChanged;
        }

        private void Property_ValueChanged(object sender, ValueChangedEventArgs e)
            => Value = (double)e.NewValue == valueToTest;
    }
}