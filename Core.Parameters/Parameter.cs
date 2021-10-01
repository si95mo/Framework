using Core.Converters;
using System;
using System.Collections.Generic;

namespace Core.Parameters
{
    /// <summary>
    /// Describe a generic parameter.
    /// See also <see cref="IParameter{T}"/>
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Parameter{T}"/></typeparam>
    [Serializable]
    public abstract class Parameter<T> : IParameter<T>
    {
        protected string code;
        protected T value;
        protected string measureUnit;
        protected string format;

        protected List<IProperty> subscribers;
        protected IConverter<T, T> converter;

        protected object objectLock = new object();
        protected EventHandler<ValueChangedEventArgs> ValueChangedHandler;

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

            value = default;

            subscribers = new List<IProperty>();
            ValueChanged += PropagateValues;
        }

        /// <summary>
        /// The <see cref="Parameter{T}"/> value
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

        /// <summary>
        /// The <see cref="Parameter{T}"/> value as <see cref="object"/>
        /// </summary>
        public virtual object ValueAsObject
        {
            get => value;
            set => this.value = (T)value;
        }

        public virtual Type Type => typeof(T);

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
        public event EventHandler<ValueChangedEventArgs> ValueChanged
        {
            add
            {
                lock (objectLock)
                {
                    ValueChangedHandler += value;
                }
            }

            remove
            {
                lock (objectLock)
                {
                    ValueChangedHandler -= value;
                }
            }
        }

        /// <summary>
        /// On value changed event
        /// </summary>
        /// <param name="e">The <see cref="ValueChangedEventArgs"/></param>
        protected virtual void OnValueChanged(ValueChangedEventArgs e)
        {
            ValueChangedHandler?.Invoke(this, e);
        }

        /// <summary>
        /// Connects an <see cref="IProperty{T}"/> to another
        /// in order to propagate its value;
        /// </summary>
        /// <param name="channel">The destination <see cref="IProperty{T}"/></param>
        public void ConnectTo(IProperty channel)
        {
            channel.ValueAsObject = value;
            subscribers.Add(channel);
        }

        /// <summary>
        /// Connects an <see cref="IParameter"/> to another
        /// in order to propagate its value converted.
        /// See also <see cref="ConnectTo(IParameter{T})"/>
        /// </summary>
        /// <param name="channel">The destination <see cref="IParameter"/></param>
        /// <param name="converter">The <see cref="IConverter"/></param>
        public void ConnectTo(IProperty channel, IConverter converter)
        {
            converter.Connect(this, channel);
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
            subscribers.ForEach(x => x.ValueAsObject = Value);
        }
    }
}