﻿using Core;
using System;

namespace Hardware
{
    /// <summary>
    /// Implement an analog input channel.
    /// See also <see cref="Channel{T}"/> and <see cref="IChannel{T}"/>
    /// </summary>
    public class AnalogOutput : Channel<double>, IAnalogOutput
    {
        /// <summary>
        /// The <see cref="AnalogInput"/> value;
        /// </summary>
        public new double Value
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
        /// Create a new instance of <see cref="AnalogOutput"/>
        /// </summary>
        public AnalogOutput() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Create a new instance of <see cref="AnalogOutput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="format">The format</param>
        /// <param name="measureUnit">The measure unit</param>
        public AnalogOutput(string code, string measureUnit = "", string format = "0.0") : base(code)
        {
            this.measureUnit = measureUnit;
            this.format = format;
        }
    }
}