using Core;
using Core.Conditions;
using Core.Converters;
using Diagnostic;
using System;
using System.Collections.Generic;

namespace Hardware
{
    /// <summary>
    /// Provides factory methods for <see cref="IChannel{T}"/>
    /// </summary>
    public class Channel
    {
        /// <summary>
        /// Factory method that create a new <see cref="IChannel"/> based on <paramref name="type"/> (with no value). See also <see cref="New(Type, string)"/>
        /// </summary>
        /// <param name="type">The <see cref="string"/> that represent the <see cref="System.Type"/> of the <see cref="IChannel"/> to create</param>
        /// <param name="code">The code of the new <see cref="IChannel"/></param>
        /// <returns>
        /// The created <see cref="IChannel"/> (with the actual type that will be the one specified by <paramref name="type"/>,
        /// or <see langword="null"/> if operation failed)
        /// </returns>
        public static IChannel New(string type, string code)
            => New(Type.GetType(type), code);

        /// <summary>
        /// Factory method that create a new <see cref="IChannel"/> based on <paramref name="type"/> (with no value). See also <see cref="New(string, string)"/>
        /// </summary>
        /// <param name="type">The <see cref="System.Type"/> of the <see cref="IChannel"/> to create</param>
        /// <param name="code">The code of the new <see cref="IChannel"/></param>
        /// <returns>
        /// The created <see cref="IChannel"/> (with the actual type that will be the one specified by <paramref name="type"/>,
        /// or <see langword="null"/> if operation failed)
        /// </returns>
        public static IChannel New(Type type, string code)
        {
            IChannel channel;
            switch (type)
            {
                case Type t when t == typeof(AnalogInput):
                    channel = new AnalogInput(code);
                    break;

                case Type t when t == typeof(AnalogOutput):
                    channel = new AnalogOutput(code);
                    break;

                case Type t when t == typeof(DigitalInput):
                    channel = new DigitalInput(code);
                    break;

                case Type t when t == typeof(DigitalOutput):
                    channel = new DigitalOutput(code);
                    break;

                case Type t when t == typeof(MultiSampleAnalogInput):
                    channel = new MultiSampleAnalogInput(code);
                    break;

                default:
                    channel = null;
                    break;
            }

            return channel;
        }
    }

    /// <summary>
    /// Describe a generic channel.
    /// See also <see cref="IChannel{T}"/>
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Channel{T}"/></typeparam>
    public abstract class Channel<T> : Channel, IChannel<T>
    {
        /// <summary>
        /// The value
        /// </summary>
        private T value;

        /// <summary>
        /// The subscribers
        /// </summary>
        private List<IProperty> subscribers;

        /// <summary>
        /// The object lock
        /// </summary>
        private object eventLock = new object();

        public event EventHandler<ValueChangedEventArgs> ValueChanged;
        public event EventHandler<ValueSetEventArgs> ValueSet;

        #region IChannel implementation

        /// <summary>
        /// The <see cref="Channel{T}"/> code
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// The <see cref="Channel{T}"/> value
        /// </summary>
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
                    if (WriteEnable != null)
                    {
                        if (WriteEnable.Value)
                        {
                            UpdateValue(value);
                        }
                        else
                        {
                            Logger.Warn($"Attempting to write {Code} with write disabled");
                        }
                    }
                    else
                    {
                        UpdateValue(value);
                    }

                    OnValueSet(new ValueSetEventArgs(Value));
                }
            }
        }

        /// <summary>
        /// Update the <see cref="Channel{T}"/> <see cref="Value"/>
        /// </summary>
        /// <param name="value">The new value</param>
        /// <remarks>This method also invoke the <see cref="OnValueChanged(ValueChangedEventArgs)"/> handler if needed</remarks>
        private void UpdateValue(T value)
        {
            if (!value.Equals(this.value))
            {
                object oldValue = this.value;
                this.value = value;

                OnValueChanged(new ValueChangedEventArgs(oldValue, this.value));
            }
        }

        /// <summary>
        /// The <see cref="Channel{T}"/> value as <see cref="object"/>
        /// </summary>
        public object ValueAsObject
        {
            get => Value;
            set => Value = (T)value;
        }

        /// <summary>
        /// The <see cref="Channel{T}"/> measure unit
        /// </summary>
        public string MeasureUnit { get; set; }

        /// <summary>
        /// The <see cref="Channel{T}"/> format
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// The <see cref="Type"/>
        /// </summary>
        public Type Type => GetType();

        public List<string> Tags { get; set; }
        public ICondition WriteEnable { get; set; }
        public ChannelType ChannelType { get; protected set; }
        public bool ReadOnly => ChannelType == ChannelType.AnalogInput || ChannelType == ChannelType.AnalogInput || ChannelType == ChannelType.MultiSampleAnalogInput;
        public string Description { get; set; }
        public string Symbolic { get; set; }

        #endregion IChannel implementation

        #region Constructors

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
            Code = code;
            Tags = new List<string>();

            value = default;

            subscribers = new List<IProperty>();
            Tags = new List<string>();

            ValueChanged += PropagateValues;
        }

        /// <summary>
        /// Initialize the class attributes
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        protected Channel(string code, string measureUnit, string format) : this(code)
        {
            MeasureUnit = measureUnit;
            Format = format;
            Description = string.Empty;
            Symbolic = string.Empty;
        }

        /// <summary>
        /// Initialize the class attributes
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        protected Channel(string code, string measureUnit, string format, IResource resource) : this(code, measureUnit, format)
        {
            resource.Channels.Add(this);
        }

        #endregion Constructors

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
        /// Connects an <see cref="IChannel"/> to another in order to propagate its value;
        /// </summary>
        /// <param name="channel">The destination <see cref="IChannel"/></param>
        public void ConnectTo(IProperty channel)
        {
            channel.ValueAsObject = Value;
            subscribers.Add(channel);
        }

        /// <summary>
        /// Connects an <see cref="IChannel"/> to another in order to propagate its value converted. See also <see cref="ConnectTo(IProperty)"/>
        /// </summary>
        /// <param name="channel">The destination <see cref="IChannel"/></param>
        /// <param name="converter">The <see cref="IConverter{TIn, TOut}"/></param>
        public void ConnectTo(IProperty channel, IConverter converter)
        {
            converter.Connect(this, channel);
        }

        /// <summary>
        /// <see cref="ValueChanged"/> event handler that manages the propagation of the values to subscribers. See <see cref="ConnectTo(IProperty)"/>
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="ValueChangedEventArgs"/></param>
        protected virtual void PropagateValues(object sender, ValueChangedEventArgs e)
        {
            subscribers.ForEach(x => x.ValueAsObject = Value);
        }

        /// <summary>
        /// Give a textual description of the <see cref="Channel{T}"/>
        /// </summary>
        /// <returns>The textual description</returns>
        public override string ToString()
        {
            string description;

            if (double.TryParse(value.ToString(), out double result))
                description = $"{result.ToString(Format)}{MeasureUnit}";
            else
                description = $"{value}{MeasureUnit}";

            return description;
        }
    }
}