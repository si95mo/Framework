using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardware
{
    /// <summary>
    /// Handles the property value changed event.
    /// See alse <see cref="EventArgs"/>
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

    public abstract class Channel : IChannel
    {
        protected object value;
        protected List<IChannel> subscribedChannels;

        /// <summary>
        /// Initialize the class attributes with 
        /// default parameters
        /// </summary>
        protected Channel()
        {
            subscribedChannels = new List<IChannel>();
            ValueChanged += PropagateValues;
        }

        /// <summary>
        /// The <see cref="Channel"/> value
        /// </summary>
        public object Value 
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
        /// The <see cref="ValueChanged"/> event handler
        /// for the <see cref="Value"/> property
        /// </summary>
        public event EventHandler<ValueChangedEventArgs> ValueChanged;

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
        public void ConnectTo(IChannel channel)
            => subscribedChannels.Add(channel);

        /// <summary>
        /// <see cref="ValueChanged"/> event handler that manages
        /// the propagation of the values to subscribers.
        /// See <see cref="ConnectTo(IChannel)"/>
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="ValueChangedEventArgs"/></param>
        private void PropagateValues(object sender, ValueChangedEventArgs e)
        {
            subscribedChannels.ForEach(x => x.Value = Value);
        }
    }
}
