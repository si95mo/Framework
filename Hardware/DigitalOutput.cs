using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardware
{
    /// <summary>
    /// Implement a digital output channel.
    /// See also <see cref="Channel{T}"/> and <see cref="IChannel{T}"/>
    /// </summary>
    public class DigitalOutput : Channel<bool>
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
        /// <param name="code">The code</param>
        public DigitalOutput(string code) : base(code)
        { }

        /// <summary>
        /// Return a description of the object
        /// See also <see cref="object.ToString()"/>
        /// </summary>
        /// <returns>The description of the object</returns>
        public override string ToString()
        {
            string description = $"{value}";

            return description;
        }
    }
}
