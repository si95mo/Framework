namespace Core.Parameters
{
    /// <summary>
    /// Implement a <see cref="NumericParameter"/> that is the division of 2 <see cref="NumericParameter"/>
    /// </summary>
    /// <remarks>If a division by 0 is done at run-time, <see cref="NumericParameter.Value"/> of the instance 
    /// should be <see cref="double.NegativeInfinity"/> or <see cref="double.PositiveInfinity"/>. <br/>
    /// Consider the using <see cref="double.IsNegativeInfinity(double)"/> or <see cref="double.IsPositiveInfinity(double)"/>
    /// (applies also in caso of <see cref="double.NaN"/> used with <see cref="double.IsNaN(double)"/>)</remarks>
    public class DivisionParameter : NumericParameter
    {
        /// <summary>
        /// Create a new instance of <see cref="DivisionParameter"/>
        /// </summary>
        /// <param name="firstParameter">The first <see cref="NumericParameter"/></param>
        /// <param name="secondParameter">The second <see cref="NumericParameter"/></param>
        /// <remarks>The value will be <paramref name="firstParameter"/> - <paramref name="secondParameter"/></remarks>
        public DivisionParameter(NumericParameter firstParameter, NumericParameter secondParameter) : base($"{firstParameter.Code}.Div.{secondParameter.Code}")
        {
            MeasureUnit = $"{firstParameter.MeasureUnit}*{secondParameter.MeasureUnit}";
            Format = firstParameter.Format;

            Value = firstParameter.Value / secondParameter.Value;

            firstParameter.ValueChanged += (s, e) => Value = firstParameter.Value / secondParameter.Value;
            secondParameter.ValueChanged += (s, e) => Value = firstParameter.Value / secondParameter.Value;
        }
    }
}
