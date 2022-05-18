using System;

namespace Core.Parameters
{
    /// <summary>
    /// Describe a generic parameter
    /// </summary>
    public interface IParameter : IProperty
    {
        /// <summary>
        /// Connect a <see cref="IProperty"/> to propagate the value
        /// </summary>
        /// <param name="property">The <see cref="IProperty"/> to connect</param>
        /// <param name="converter">The <see cref="IConverter"/></param>
        void ConnectTo(IProperty property, IConverter converter);

        /// <summary>
        /// Connect a <see cref="IProperty"/> to propagate the value
        /// </summary>
        /// <param name="property">The <see cref="IProperty"/> to connect</param>
        void ConnectTo(IProperty property);

        /// <summary>
        /// The value changed event. See <see cref="ValueChangedEventArgs"/>
        /// </summary>
        event EventHandler<ValueChangedEventArgs> ValueChanged;

        /// <summary>
        /// The <see cref="IParameter"/> description
        /// </summary>
        string Description { get; set; }
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
    }
}