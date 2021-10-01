using Core;

namespace Hardware
{
    /// <summary>
    /// Describe a generic (hardware) channel
    /// </summary>
    public interface IChannel : IProperty
    {
        /// <summary>
        /// Connect a <see cref="IProperty"/> to propagate the value
        /// </summary>
        /// <param name="property">The <see cref="IProperty"/> to connect</param>
        /// <param name="converter">The <see cref="IConverter"/></param>
        void ConnectTo(IProperty property, IConverter converter);
    }

    /// <summary>
    /// Describe a generic (hardware) channel
    /// with a defined type of value
    /// </summary>
    /// <typeparam name="T">The type of value</typeparam>
    public interface IChannel<T> : IProperty<T>, IChannel
    {
        /// <summary>
        /// The <see cref="IChannel"/> code
        /// </summary>
        new T Value
        {
            get;
            set;
        }
    }
}