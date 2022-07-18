namespace Core.Parameters
{
    /// <summary>
    /// Implement a <see cref="NumericParameter"/> that is the sum of 2 <see cref="NumericParameter"/>
    /// </summary>
    public class SumParameter : NumericParameter
    {
        /// <summary>
        /// Create a new instance of <see cref="SumParameter"/>
        /// </summary>
        /// <param name="firstParameter">The first <see cref="NumericParameter"/></param>
        /// <param name="secondParameter">The second <see cref="NumericParameter"/></param>
        public SumParameter(NumericParameter firstParameter, NumericParameter secondParameter) : base($"{firstParameter.Code}.Plus.{secondParameter.Code}")
        {
            MeasureUnit = firstParameter.MeasureUnit;
            Format = firstParameter.Format;

            Value = firstParameter.Value + secondParameter.Value;

            firstParameter.ValueChanged += (s, e) => Value = firstParameter.Value + secondParameter.Value;
            secondParameter.ValueChanged += (s, e) => Value = firstParameter.Value + secondParameter.Value;
        }
    }
}
