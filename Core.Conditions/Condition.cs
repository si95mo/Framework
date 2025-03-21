﻿using System;
using System.Collections.Generic;

namespace Core.Conditions
{
    /// <summary>
    /// Define a generic condition
    /// </summary>
    public abstract class Condition : ICondition
    {
        private string code;
        private bool value;
        private List<IProperty> subscribers;

        private EventHandler<ValueChangedEventArgs> ValueChangedHandler;
        private object eventLock = new object();

        public string Code => code;

        public object ValueAsObject
        {
            get => value;
            set => this.value = (bool)value;
        }

        public Type Type => typeof(Condition);

        public bool Value
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
        /// Initialize the <see cref="Condition"/>
        /// </summary>
        /// <param name="code">The code</param>
        protected Condition(string code)
        {
            this.code = code;

            subscribers = new List<IProperty>();
            ValueChanged += PropagateValues;
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged
        {
            add
            {
                lock (eventLock)
                    ValueChangedHandler += value;
            }
            remove
            {
                lock (eventLock)
                    ValueChangedHandler -= value;
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

        public void ConnectTo(IProperty property)
        {
            property.ValueAsObject = value;
            subscribers.Add(property);
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

        /// <summary>
        /// Create a new <see cref="ICondition"/> that will be <see langword="true"/>
        /// when the actual <see cref="Condition"/> is <see langword="true"/>
        /// </summary>
        /// <returns>The new <see cref="ICondition"/></returns>
        public abstract ICondition IsTrue();

        /// <summary>
        /// Create a new <see cref="ICondition"/> that will be <see langword="true"/>
        /// when the actual <see cref="Condition"/> is <see langword="false"/>
        /// </summary>
        /// <returns>The new <see cref="ICondition"/></returns>
        public abstract ICondition IsFalse();
    }
}