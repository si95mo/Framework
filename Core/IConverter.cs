namespace Core
{
    public interface IConverter
    {
        /// <summary>
        /// Connect two <see cref="IProperty"/> in order to
        /// propagate the converted value
        /// </summary>
        /// <param name="sourceParameter">The source <see cref="IParameter"/></param>
        /// <param name="destinationParameter">The destination <see cref="IParameter"/></param>
        void Connect(IProperty sourceParameter, IProperty destinationParameter);
    }
}