using System;

namespace Core
{
    /// <summary>
    /// Describve a generic failure.
    /// See also <see cref="IFailure"/>
    /// </summary>
    public class Failure : IFailure
    {
        private string description;
        private DateTime timestamp;

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
            set => timestamp = value;
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
            description = "";
            timestamp = new DateTime();
        }

        /// <summary>
        /// Create a new instance of <see cref="Failure"/>
        /// </summary>
        /// <param name="description">The description</param>
        /// <param name="timestamp">The timestamp</param>
        public Failure(string description, DateTime timestamp)
        {
            this.description = description;
            this.timestamp = timestamp;
        }

        /// <summary>
        /// Create a new instance of <see cref="Failure"/>
        /// </summary>
        /// <param name="description">The description</param>
        public Failure(string description) : this(description, DateTime.Now)
        { }

        /// <summary>
        /// Clear the <see cref="Failure"/>, 
        /// resetting it to default values
        /// </summary>
        public void Clear()
        {
            description = "";
            timestamp = new DateTime();
        }

        /// <summary>
        /// Return a description of the object
        /// See also <see cref="object.ToString()"/>
        /// </summary>
        /// <returns>The description of the object</returns>
        override
        public string ToString()
        {
            string description = $"{timestamp.ToString("yyyy/MM/dd-HH:mm:ss:fff")}; {this.description}";

            return description;
        }
    }
}
