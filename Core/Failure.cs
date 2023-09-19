using System;

namespace Core
{
    /// <summary>
    /// Describe a generic failure.
    /// See also <see cref="IFailure"/>
    /// </summary>
    public class Failure : IFailure
    {
        private string description;
        private DateTime timestamp;

        /// <summary>
        /// The object lock
        /// </summary>
        protected object objectLock = new object();

        /// <summary>
        /// The value changed <see cref="EventHandler"/>
        /// </summary>
        protected EventHandler<ValueChangedEventArgs> ValueChangedHandler;

        /// <summary>
        /// The <see cref="Failure"/> description
        /// </summary>
        public string Description
        {
            get => description;
            set => description = value;
        }

        /// <summary>
        /// The <see cref="Failure"/> timestamp
        /// </summary>
        public DateTime Timestamp
        {
            get => timestamp;
            set
            {
                if (!value.Equals(timestamp))
                {
                    DateTime oldTimestamp = timestamp;
                    timestamp = value;
                    OnValueChanged(new ValueChangedEventArgs(oldTimestamp, timestamp));
                }
            }
        }

        /// <summary>
        /// The <see cref="Failure"/> default value
        /// </summary>
        public IFailure Default => new Failure();

        /// <summary>
        /// Create a new instance of <see cref="Failure"/>
        /// </summary>
        public Failure()
        {
            description = string.Empty;
            Timestamp = new DateTime();
        }

        /// <summary>
        /// Create a new instance of <see cref="Failure"/>
        /// </summary>
        /// <param name="description">The description</param>
        /// <param name="timestamp">The timestamp</param>
        public Failure(string description, DateTime timestamp)
        {
            this.description = description;
            Timestamp = timestamp;
        }

        /// <summary>
        /// Create a new instance of <see cref="Failure"/>
        /// </summary>
        /// <param name="description">The description</param>
        public Failure(string description) : this(description, DateTime.Now)
        { }

        /// <summary>
        /// Create a new instance of <see cref="Failure"/>
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> occurred</param>
        public Failure(Exception ex) : this(ex.Message, DateTime.Now)
        { }

        /// <summary>
        /// The <see cref="EventHandler"/> for the
        /// value changed event
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
        /// Clear the <see cref="Failure"/>,
        /// resetting it to default values
        /// </summary>
        public void Clear()
        {
            description = string.Empty;
            Timestamp = new DateTime();
        }

        /// <summary>
        /// Return a description of the object
        /// See also <see cref="object.ToString()"/>
        /// </summary>
        /// <returns>The description of the object</returns>
        public override string ToString()
        {
            string description = $"{timestamp:yyyy/MM/dd-HH:mm:ss:fff}; {this.description}";
            return description;
        }
    }
}