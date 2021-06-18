using System;
using System.Collections.Generic;

namespace Hardware
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
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
    }

    /// <summary>
    /// Describe a generic channel.
    /// See also <see cref="IChannel{T}"/>
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Channel{T}"/></typeparam>
    public abstract class Channel<T> : IChannel<T>
    {
        protected string code;
        protected T value;
        protected string measureUnit;
        protected string format;
        protected List<IChannel<T>> subscribers;

        /// <summary>
        /// Initialize the class attributes with 
        /// default parameters
        /// </summary>
        protected Channel() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Initialize the class attributes with 
        /// default parameters
        /// </summary>
        /// <param name="code">The code</param>
        protected Channel(string code)
        {
            this.code = code;

            subscribers = new List<IChannel<T>>();
            ValueChanged += PropagateValues;
        }

        /// <summary>
        /// The <see cref="Channel"/> code
        /// </summary>
        public string Code => code;

        /// <summary>
        /// The <see cref="Channel"/> value
        /// </summary>
        public virtual T Value
        {
            get => value;
            set
            {
                if (!value.Equals(this.value))
                {
                    object oldValue = this.value;
                    this.value = value;
                    OnValueChanged(new ValueChangedEventArgs(oldValue, this.value));
                }
            }
        }

        protected virtual string MeasureUnit 
        { 
            get => measureUnit; 
            set => measureUnit = value; 
        }
        protected virtual string Format 
        {
            get => format; 
            set => format = value; 
        }

        /// <summary>
        /// The <see cref="ValueChanged"/> event handler
        /// for the <see cref="Value"/> property
        /// </summary>
        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        /// <summary>
        /// On value changed event
        /// </summary>
        /// <param name="e">The <see cref="ValueChangedEventArgs"/></param>
        protected virtual void OnValueChanged(ValueChangedEventArgs e)
        {
            if (ValueChanged != null)
                ValueChanged(this, e);
        }

        /// <summary>
        /// Connects an <see cref="IChannel"/> to another
        /// in order to propagate its value;
        /// </summary>
        /// <param name="channel">The destination <see cref="IChannel"/></param>
        public void ConnectTo(IChannel<T> channel)
        {
            channel.Value = value;
            subscribers.Add(channel);
        }

        /// <summary>
        /// <see cref="ValueChanged"/> event handler that manages
        /// the propagation of the values to subscribers.
        /// See <see cref="ConnectTo(IChannel)"/>
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="ValueChangedEventArgs"/></param>
        private void PropagateValues(object sender, ValueChangedEventArgs e)
        {
            subscribers.ForEach(x => x.Value = Value);
        }
    }
}
