using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
    }

    /// <summary>
    /// Describe a generic parameter.
    /// See also <see cref="IParameter{T}"/>
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Parameter{T}"/></typeparam>
    public abstract class Parameter<T> : IParameter<T>
    {
        protected string code;
        protected T value;
        protected string measureUnit;
        protected string format;
        protected List<IParameter<T>> subscribers;

        /// <summary>
        /// Initialize the class attributes with 
        /// default parameters
        /// </summary>
        protected Parameter() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Initialize the class attributes with 
        /// default parameters
        /// </summary>
        /// <param name="code">The code</param>
        protected Parameter(string code)
        {
            this.code = code;

            subscribers = new List<IParameter<T>>();
            ValueChanged += PropagateValues;
        }

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

        /// <summary>
        /// The <see cref="Parameter{T}"/> measure unit
        /// </summary>
        protected virtual string MeasureUnit 
        {
            get => measureUnit;
            set => measureUnit = value;
        }

        /// <summary>
        /// The <see cref="Parameter{T}"/> format
        /// </summary>
        protected virtual string Format
        {
            get => format;
            set => format = value;
        }

        /// <summary>
        /// The <see cref="Parameter{T}"/> code
        /// </summary>
        public string Code => code;

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
        public void ConnectTo(IParameter<T> channel)
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
