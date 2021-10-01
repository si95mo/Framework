namespace Core
{
    /// <summary>
    /// Define a generic converter
    /// </summary>
    public interface IConverter
    {
        /// <summary>
        /// Connect two <see cref="IProperty"/> in order to
        /// propagate the converted value
        /// </summary>
        /// <param name="sourceParameter">The source <see cref="IProperty"/></param>
        /// <param name="destinationParameter">The destination <see cref="IProperty"/></param>
        void Connect(IProperty sourceParameter, IProperty destinationParameter);
    }
}