using Core;
using Core.Converters;
using System;
using System.Collections.Generic;

namespace Hardware
{
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

        protected List<IProperty<T>> subscribers;

        protected object objectLock = new object();

        protected EventHandler<ValueChangedEventArgs> ValueChangedHandler;

        /// <summary>
        /// The <see cref="Type"/>
        /// </summary>
        public Type Type => typeof(T);

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

            value = default;

            subscribers = new List<IProperty<T>>();
            ValueChanged += PropagateValues;
        }

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
        /// The <see cref="Channel{T}"/> code
        /// </summary>
        public string Code => code;

        /// <summary>
        /// The <see cref="Channel{T}"/> value
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
        /// The <see cref="Channel{T}"/> value as <see cref="object"/>
        /// </summary>
        public virtual object ValueAsObject
        {
            get => value;
            set => this.value = (T)value;
        }

        /// <summary>
        /// The <see cref="Channel{T}"/> measure unit
        /// </summary>
        public virtual string MeasureUnit
        {
            get => measureUnit;
            set => measureUnit = value;
        }

        /// <summary>
        /// The <see cref="Channel{T}"/> format
        /// </summary>
        public virtual string Format
        {
            get => format;
            set => format = value;
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
        /// Connects an <see cref="IChannel"/> to another
        /// in order to propagate its value;
        /// </summary>
        /// <param name="channel">The destination <see cref="IChannel"/></param>
        public void ConnectTo(IProperty<T> channel)
        {
            channel.Value = value;
            subscribers.Add(channel);
        }

        /// <summary>
        /// Connects an <see cref="IChannel"/> to another
        /// in order to propagate its value converted.
        /// See also <see cref="ConnectTo(IChannel{T})"/>
        /// </summary>
        /// <param name="channel">The destination <see cref="IChannel"/></param>
        /// <param name="converter">The <see cref="IConverter{TIn, TOut}"/></param>
        public void ConnectTo(IProperty<T> channel, IConverter<T, T> converter)
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
        protected virtual void PropagateValues(object sender, ValueChangedEventArgs e)
        {
            subscribers.ForEach(x => x.Value = Value);
        }
    }
}