namespace Core.Parameters
{
    /// <summary>
    /// Implement a <see cref="NumericParameter"/> that is the multiplication of 2 <see cref="NumericParameter"/>
    /// </summary>
    public class MultiplicationParameter : NumericParameter
    {
        /// <summary>
        /// Create a new instance of <see cref="MultiplicationParameter"/>
        /// </summary>
        /// <param name="firstParameter">The first <see cref="NumericParameter"/></param>
        /// <param name="secondParameter">The second <see cref="NumericParameter"/></param>
        public MultiplicationParameter(NumericParameter firstParameter, NumericParameter secondParameter) : base($"{firstParameter.Code}.Mul.{secondParameter.Code}")
        {
            MeasureUnit = $"{firstParameter.MeasureUnit}*{secondParameter.MeasureUnit}";
            Format = firstParameter.Format;

            Value = firstParameter.Value * secondParameter.Value;

            firstParameter.ValueChanged += (s, e) => Value = firstParameter.Value * secondParameter.Value;
            secondParameter.ValueChanged += (s, e) => Value = firstParameter.Value * secondParameter.Value;
        }
    }
}