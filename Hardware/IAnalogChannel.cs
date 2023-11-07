namespace Hardware
{
    /// <summary>
    /// Define a basic interface for analog <see cref="IChannel{T}"/>
    /// </summary>
    public interface IAnalogChannel : IChannel<double>
    {
        /// <summary>
        /// The measure unit
        /// </summary>
        string MeasureUnit { get; set; }
    }
}