namespace Core.Parameters
{
    /// <summary>
    /// Implement a <see cref="NumericParameter"/> that is the difference of 2 <see cref="NumericParameter"/>
    /// </summary>
    public class DifferenceParameter : NumericParameter
    {
        /// <summary>
        /// Create a new instance of <see cref="DifferenceParameter"/>
        /// </summary>
        /// <param name="firstParameter">The first <see cref="NumericParameter"/></param>
        /// <param name="secondParameter">The second <see cref="NumericParameter"/></param>
        /// <remarks>The value will be <paramref name="firstParameter"/> - <paramref name="secondParameter"/></remarks>
        public DifferenceParameter(NumericParameter firstParameter, NumericParameter secondParameter) : base($"{firstParameter.Code}.Sub.{secondParameter.Code}")
        {
            MeasureUnit = firstParameter.MeasureUnit;
            Format = firstParameter.Format;

            Value = firstParameter.Value - secondParameter.Value;

            firstParameter.ValueChanged += (s, e) => Value = firstParameter.Value - secondParameter.Value;
            secondParameter.ValueChanged += (s, e) => Value = firstParameter.Value - secondParameter.Value;
        }
    }
}
