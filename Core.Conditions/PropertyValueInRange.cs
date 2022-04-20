namespace Core.Conditions
{
    /// <summary>
    /// Implement an <see cref="ICondition"/> that will be <see langword="true"/>
    /// when a property value is in the passed range
    /// </summary>
    public class PropertyValueInRange : FlyweightCondition
    {
        private double minimum, maximum;
        private bool isMinimumExcluded, isMaximumExcluded;

        /// <summary>
        /// Create a new instance of <see cref="PropertyValueInRange"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="property">The <see cref="IProperty{T}"/> of which test the value</param>
        /// <param name="minimum">The minimum value of the range</param>
        /// <param name="maximum">The maximum value of the range</param>
        /// <param name="isMinimumExcluded">
        /// <see langword="true"/> if <paramref name="minimum"/> is excluded from the range, <see langword="false"/> otherwise
        /// </param>
        /// <param name="isMaximumExcluded">
        /// <see langword="true"/> if <paramref name="maximum"/> is excluded from the range, <see langword="false"/> otherwise
        /// </param>
        public PropertyValueInRange(string code, IProperty<double> property, double minimum, double maximum,
            bool isMinimumExcluded = false, bool isMaximumExcluded = false) : base(code)
        {
            this.minimum = minimum;
            this.maximum = maximum;
            this.isMinimumExcluded = isMinimumExcluded;
            this.isMaximumExcluded = isMaximumExcluded;

            property.ValueChanged += Property_ValueChanged;
        }

        private void Property_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            double value = (double)e.NewValue;

            bool result = isMinimumExcluded ? value > minimum : value >= minimum;

            if (result)
                result = isMaximumExcluded ? value < maximum : value <= maximum;

            Value = result;
        }
    }
}