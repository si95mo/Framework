using System;

namespace Core
{
    /// <summary>
    /// Describe a generic failure.
    /// See also <see cref="IFailure"/>
    /// </summary>
    public class Failure : IFailure
    {
        private DateTime timestamp;

        /// <summary>
        /// The lock <see cref="object"/>
        /// </summary>
        protected object Sync = new object();

        #region IFailure properties implementation

        public event EventHandler<ValueChangedEventArgs> FailureChanged;

        public string Description { get; set; }

        public DateTime Timestamp
        {
            get => timestamp;
            set
            {
                if (!value.Equals(timestamp))
                {
                    lock (Sync)
                    {
                        IFailure oldFailure = FromFailure(this);

                        DateTime oldTimestamp = timestamp;
                        timestamp = value;

                        FailureChanged?.Invoke(this, new ValueChangedEventArgs(oldFailure, this));
                    }
                }
            }
        }

        public Exception Exception { get; set; }

        /// <summary>
        /// The <see cref="Failure"/> default value
        /// </summary>
        public IFailure Default => new Failure();

        #endregion IFailure properties implementation

        #region Constructors

        /// <summary>
        /// Create a new instance of <see cref="Failure"/>
        /// </summary>
        public Failure()
        {
            Description = string.Empty;
            Timestamp = new DateTime();
            Exception = default;
        }

        /// <summary>
        /// Create a new instance of <see cref="Failure"/>
        /// </summary>
        /// <param name="description">The description</param>
        /// <param name="timestamp">The timestamp</param>
        public Failure(string description, DateTime timestamp)
        {
            Description = description;
            Timestamp = timestamp;
            Exception = default;
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
        /// <param name="ex">The <see cref="System.Exception"/> occurred</param>
        public Failure(Exception ex) : this(ex, DateTime.Now)
        { }

        /// <summary>
        /// Create a new instance of <see cref="Failure"/>
        /// </summary>
        /// <param name="ex">The <see cref="System.Exception"/> occurred</param>
        /// <param name="timestamp">The timestamp</param>
        public Failure(Exception ex, DateTime timestamp) : this(ex.Message, timestamp)
        {
            Exception = ex;
        }

        #endregion Constructors

        #region IFailure methods implementation

        /// <summary>
        /// Clear the <see cref="Failure"/>, resetting it to default values
        /// </summary>
        public void Clear()
        {
            lock (Sync)
            {
                Description = string.Empty;
                Exception = default;
                Timestamp = new DateTime(); // ValueChanged
            }
        }

        public void Update(string description, Exception ex = default)
        {
            lock (Sync)
            {
                Description = description;
                Exception = ex;
                Timestamp = DateTime.Now; // ValueChanged
            }
        }

        #endregion IFailure methods implementation

        /// <summary>
        /// Return a description of the object. See also <see cref="object.ToString()"/>
        /// </summary>
        /// <returns>The description of the object</returns>
        public override string ToString()
        {
            string description = $"{timestamp:yyyy/MM/dd-HH:mm:ss:fff}; {Description}";
            return description;
        }

        #region Factory methods

        /// <summary>
        /// Create a new <see cref="IFailure"/> from an <see cref="Exception"/>
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/></param>
        /// <returns>The new <see cref="IFailure"/></returns>
        public static IFailure FromException(Exception ex)
        {
            Failure failure = new Failure(ex);
            return failure;
        }

        /// <summary>
        /// Create a new <see cref="IFailure"/> from an error message
        /// </summary>
        /// <param name="message">The error message</param>
        /// <returns>The new <see cref="IFailure"/></returns>
        public static IFailure FromErrorMessage(string message)
        {
            Failure failure = new Failure(message);
            return failure;
        }

        /// <summary>
        /// Create a new <see cref="IFailure"/> from an existing one
        /// </summary>
        /// <param name="failure">The original <see cref="IFailure"/></param>
        /// <returns>The new <see cref="IFailure"/></returns>
        public static IFailure FromFailure(IFailure failure)
        {
            Failure copy = new Failure(failure.Description, failure.Timestamp);
            if (failure.Exception != default)
            {
                copy.Exception = failure.Exception;
            }

            return copy;
        }

        #endregion Factory methods
    }
}