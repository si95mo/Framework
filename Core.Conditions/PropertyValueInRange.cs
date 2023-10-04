namespace Core.Conditions
{
    /// <summary>
    /// Implement an <see cref="ICondition"/> that will be <see langword="true"/>
    /// when a property value is in the passed range
    /// </summary>
    public class PropertyValueInRange : FlyweightCondition
    {
        private double minimumValue, maximumValue;
        private IProperty<double> minimumProperty, maximumProperty;
        private bool isMinimumExcluded, isMaximumExcluded;

        /// <summary>
        /// Create a new instance of <see cref="PropertyValueInRange"/>, with max and min fixed on <paramref name="maximumValue"/> and <paramref name="minimumValue"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="property">The <see cref="IProperty{T}"/> of which test the value</param>
        /// <param name="minimumValue">The minimum value of the range</param>
        /// <param name="maximumValue">The maximum value of the range</param>
        /// <param name="isMinimumExcluded">
        /// <see langword="true"/> if <paramref name="minimumValue"/> is excluded from the range, <see langword="false"/> otherwise
        /// </param>
        /// <param name="isMaximumExcluded">
        /// <see langword="true"/> if <paramref name="maximumValue"/> is excluded from the range, <see langword="false"/> otherwise
        /// </param>
        public PropertyValueInRange(string code, IProperty<double> property, double minimumValue, double maximumValue,
            bool isMinimumExcluded = false, bool isMaximumExcluded = false) : base(code)
        {
            this.minimumValue = minimumValue;
            this.maximumValue = maximumValue;
            this.isMinimumExcluded = isMinimumExcluded;
            this.isMaximumExcluded = isMaximumExcluded;

            property.ValueChanged += BasicProperty_ValueChanged;
        }

        /// <summary>
        /// Create a new instance of <see cref="PropertyValueInRange"/>, with max and min updated based on the relative <see cref="IProperty{T}"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="property">The <see cref="IProperty{T}"/> of which test the value</param>
        /// <param name="minimumProperty">The minimum value of the range</param>
        /// <param name="maximumProperty">The maximum value of the range</param>
        /// <param name="isMinimumExcluded">
        /// <see langword="true"/> if <paramref name="minimumProperty"/> is excluded from the range, <see langword="false"/> otherwise
        /// </param>
        /// <param name="isMaximumExcluded">
        /// <see langword="true"/> if <paramref name="maximumProperty"/> is excluded from the range, <see langword="false"/> otherwise
        /// </param>
        public PropertyValueInRange(string code, IProperty<double> property, IProperty<double> minimumProperty, IProperty<double> maximumProperty,
            bool isMinimumExcluded = false, bool isMaximumExcluded = false) : base(code)
        {
            this.minimumProperty = minimumProperty;
            this.maximumProperty = maximumProperty;
            this.isMinimumExcluded = isMinimumExcluded;
            this.isMaximumExcluded = isMaximumExcluded;

            property.ValueChanged += AdvancedProperty_ValueChanged;
        }

        private void BasicProperty_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            double value = (double)e.NewValue;
            bool result = isMinimumExcluded ? value > minimumValue : value >= minimumValue;

            if (result)
            {
                result = isMaximumExcluded ? value < maximumValue : value <= maximumValue;
            }

            Value = result;
        }

        private void AdvancedProperty_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            double value = (double)e.NewValue;
            bool result = isMinimumExcluded ? value > minimumProperty.Value : value >= maximumProperty.Value;

            if (result)
            {
                result = isMaximumExcluded ? value < minimumProperty.Value : value <= maximumProperty.Value;
            }

            Value = result;
        }
    }
}