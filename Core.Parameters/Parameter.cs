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
    public abstract class Parameter<T> : IParameter<T>, IReadOnlyProperty<T>
    {
        private T value;

        /// <summary>
        /// The subscribers
        /// </summary>
        private List<IProperty> subscribers;

        /// <summary>
        /// The converter
        /// </summary>
        private IConverter<T, T> converter;

        /// <summary>
        /// The object lock
        /// </summary>
        private readonly object eventLock = new object();

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        public event EventHandler<ValueSetEventArgs> ValueSet;

        public string Description { get; set; }

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
            Code = code;

            value = default;

            subscribers = new List<IProperty>();
            ValueChanged += PropagateValues;

            Description = code;
        }

        /// <summary>
        /// The <see cref="Parameter{T}"/> value
        /// </summary>
        /// <remarks>
        /// When overriding remember to use <see cref="OnValueChanged(ValueChangedEventArgs)"/> and <see cref="OnValueSet(ValueSetEventArgs)"/>
        /// to invoke the event handlers!
        /// </remarks>
        public virtual T Value
        {
            get 
            { 
                lock (eventLock)
                {
                    return value;
                }
            }
            set
            {
                lock (eventLock)
                {
                    if (!value.Equals(this.value))
                    {
                        object oldValue = this.value;
                        this.value = value;

                        OnValueChanged(new ValueChangedEventArgs(oldValue, this.value));
                    }

                    OnValueSet(new ValueSetEventArgs(Value));
                }
            }
        }

        /// <summary>
        /// The <see cref="Parameter{T}"/> value as <see cref="object"/>
        /// </summary>
        public object ValueAsObject
        {
            get => Value;
            set => Value = (T)value;
        }

        /// <summary>
        /// The <see cref="System.Type"/>
        /// </summary>
        public Type Type => typeof(T);

        /// <summary>
        /// The <see cref="Parameter{T}"/> measure unit
        /// </summary>
        public string MeasureUnit { get; set; }

        /// <summary>
        /// The <see cref="Parameter{T}"/> format
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// The <see cref="Parameter{T}"/> code
        /// </summary>
        public string Code { get; } 

        /// <summary>
        /// On value changed event
        /// </summary>
        /// <param name="e">The <see cref="ValueChangedEventArgs"/></param>
        protected virtual void OnValueChanged(ValueChangedEventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        /// <summary>
        /// On value set event
        /// </summary>
        /// <param name="e">The <see cref="ValueSetEventArgs"/></param>
        protected virtual void OnValueSet(ValueSetEventArgs e)
        {
            ValueSet?.Invoke(this, e);
        }

        /// <summary>
        /// Connects an <see cref="IProperty{T}"/> to another
        /// in order to propagate its value;
        /// </summary>
        /// <param name="property">The destination <see cref="IProperty{T}"/></param>
        public void ConnectTo(IProperty property)
        {
            property.ValueAsObject = Value;
            subscribers.Add(property);
        }

        /// <summary>
        /// Connects an <see cref="IParameter"/> to another
        /// in order to propagate its value converted.
        /// See also <see cref="ConnectTo(IProperty)"/>
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
        /// See <see cref="ConnectTo(IProperty)"/>
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="ValueChangedEventArgs"/></param>
        private void PropagateValues(object sender, ValueChangedEventArgs e)
        {
            subscribers.ForEach(x => x.ValueAsObject = Value);
        }
    }
}