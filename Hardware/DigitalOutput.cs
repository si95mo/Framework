﻿using Core;
using System;

namespace Hardware
{
    /// <summary>
    /// Implement a digital output channel.
    /// See also <see cref="Channel{T}"/> and <see cref="IChannel{T}"/>
    /// </summary>
    public class DigitalOutput : Channel<bool>, IDigitalOutput
    {
        /// <summary>
        /// The <see cref="AnalogInput"/> value;
        /// </summary>
        public new bool Value
        {
            get => value;
            set
            {
                if (value != this.value)
                {
                    object oldValue = this.value;
                    this.value = value;
                    OnValueChanged(new ValueChangedEventArgs(oldValue, this.value));
                }
            }
        }

        /// <summary>
        /// Create a new instance of <see cref="DigitalOutput"/>
        /// </summary>
        public DigitalOutput() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Create a new instance of <see cref="DigitalOutput"/>
        /// </summary>
        /// <param name="code">The code</param>
        public DigitalOutput(string code) : base(code)
        { }
    }
}