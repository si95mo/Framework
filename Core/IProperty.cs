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

        #region OldValue casted properties

        /// <summary>
        /// The old value as <see cref="double"/>
        /// </summary>
        /// <remarks>
        /// If the conversion throws an <see cref="Exception"/>, <see cref="double.NaN"/> is returned
        /// </remarks>
        public double OldValueAsDouble
        {
            get
            {
                double value = double.NaN;
                try
                {
                    value = Convert.ToDouble(OldValue);
                }
                catch { }

                return value;
            }
        }

        /// <summary>
        /// The old value as <see cref="int"/>
        /// </summary>
        /// <remarks>
        /// If the conversion throws an <see cref="Exception"/>, <see cref="int.MinValue"/> is returned
        /// </remarks>
        public int OldValueAsInt
        {
            get
            {
                int value = int.MinValue;
                try
                {
                    value = Convert.ToInt32(OldValue);
                }
                catch { }

                return value;
            }
        }

        /// <summary>
        /// The old value as <see cref="bool"/>
        /// </summary>
        /// <remarks>
        /// If the conversion throws an <see cref="Exception"/>, <see langword="false"/> is returned
        /// </remarks>
        public bool OldValueAsBool
        {
            get
            {
                bool value = false;
                try
                {
                    value = Convert.ToBoolean(OldValue);
                }
                catch { }

                return value;
            }
        }

        /// <summary>
        /// The old value as <see cref="string"/>
        /// </summary>
        /// <remarks>
        /// If the conversion throws an <see cref="Exception"/>, <see cref="string.Empty"/> is returned
        /// </remarks>
        public string OldValueAsString
        {
            get
            {
                string value = string.Empty;
                try
                {
                    value = Convert.ToString(OldValue);
                }
                catch { }

                return value;
            }
        }

        #endregion OldValue casted properties

        /// <summary>
        /// The new value
        /// </summary>
        public readonly object NewValue;

        #region NewValue casted properties

        /// <summary>
        /// The old value as <see cref="double"/>
        /// </summary>
        /// <remarks>
        /// If the conversion throws an <see cref="Exception"/>, <see cref="double.NaN"/> is returned
        /// </remarks>
        public double NewValueAsDouble
        {
            get
            {
                double value = double.NaN;
                try
                {
                    value = Convert.ToDouble(NewValue);
                }
                catch { }

                return value;
            }
        }

        /// <summary>
        /// The old value as <see cref="int"/>
        /// </summary>
        /// <remarks>
        /// If the conversion throws an <see cref="Exception"/>, <see cref="int.MinValue"/> is returned
        /// </remarks>
        public int NewValueAsInt
        {
            get
            {
                int value = int.MinValue;
                try
                {
                    value = Convert.ToInt32(NewValue);
                }
                catch { }

                return value;
            }
        }

        /// <summary>
        /// The old value as <see cref="bool"/>
        /// </summary>
        /// <remarks>
        /// If the conversion throws an <see cref="Exception"/>, <see langword="false"/> is returned
        /// </remarks>
        public bool NewValueAsBool
        {
            get
            {
                bool value = false;
                try
                {
                    value = Convert.ToBoolean(NewValue);
                }
                catch { }

                return value;
            }
        }

        /// <summary>
        /// The old value as <see cref="string"/>
        /// </summary>
        /// <remarks>
        /// If the conversion throws an <see cref="Exception"/>, <see cref="string.Empty"/> is returned
        /// </remarks>
        public string NewValueAsString
        {
            get
            {
                string value = string.Empty;
                try
                {
                    value = Convert.ToString(NewValue);
                }
                catch { }

                return value;
            }
        }

        #endregion NewValue casted properties

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

        /// <summary>
        /// The <see cref="IProperty"/> value as <see cref="object"/>
        /// </summary>
        object ValueAsObject
        { get; set; }

        /// <summary>
        /// The <see cref="IProperty"/> wrapped
        /// value <see cref="System.Type"/>
        /// </summary>
        Type Type
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
        /// The <see cref="IProperty"/> value
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
        /// Connects an <see cref="IProperty"/> to another
        /// in order to propagate its value
        /// </summary>
        /// <param name="property">The destination <see cref="IProperty"/></param>
        void ConnectTo(IProperty property);
    }
}