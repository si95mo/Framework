namespace Core
{
    /// <summary>
    /// Describe a generic parameter
    /// </summary>
    public interface IParameter : IProperty
    {
    }

    /// <summary>
    /// Describe a generic parameter
    /// with a defined type of value
    /// </summary>
    /// <typeparam name="T">The type of value</typeparam>
    public interface IParameter<T> : IProperty<T>, IParameter
    {
        /// <summary>
        /// The <see cref="IParameter"/> value
        /// </summary>
        new T Value
        {
            get;
            set;
        }

        /// <summary>
        /// Connects an <see cref="IParameter"/> to another
        /// in order to propagate its value;
        /// </summary>
        /// <param name="channel">The destination <see cref="IParameter"/></param>
        void ConnectTo(IProperty<T> channel);
    }
}