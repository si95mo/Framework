using System;

namespace Core
{
    /// <summary>
    /// Handles the property value changed event.
    /// See also <see cref="EventArgs"/>
    /// </summary>
    public class ValueChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The old value
        /// </summary>
        public readonly object OldValue;

        /// <summary>
        /// The new value
        /// </summary>
        public readonly object NewValue;

        /// <summary>
        /// Create a new instance of <see cref="ValueChangedEventArgs"/>
        /// </summary>
        /// <param name="oldValue">The old value</param>
        /// <param name="newValue">The new value</param>
        public ValueChangedEventArgs(object oldValue, object newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    /// <summary>
    /// Describe a generic property
    /// </summary>
    public interface IProperty
    {
        /// <summary>
        /// The <see cref="IProperty"/> code
        /// </summary>
        string Code
        { get; }
    }

    /// <summary>
    /// Describe a generic parameter
    /// with a defined type of value
    /// </summary>
    /// <typeparam name="T">The type of value</typeparam>
    public interface IProperty<T> : IProperty
    {
        /// <summary>
        /// The <see cref="IProperty"/> code
        /// </summary>
        T Value
        {
            get;
            set;
        }

        /// <summary>
        /// The <see cref="ValueChanged"/> event handler
        /// for the <see cref="Value"/> property
        /// </summary>
        event EventHandler<ValueChangedEventArgs> ValueChanged;

        /// <summary>
        /// Connects an <see cref="IParameter"/> to another
        /// in order to propagate its value;
        /// </summary>
        /// <param name="channel">The destination <see cref="IParameter"/></param>
        void ConnectTo(IProperty<T> channel);
    }
}