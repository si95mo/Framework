using System;

namespace Core
{
    /// <summary>
    /// Interface that describe a generic failure.
    /// </summary>
    public interface IFailure
    {
        /// <summary>
        /// The <see cref="IFailure"/> description
        /// </summary>
        string Description
        { get; set; }

        /// <summary>
        /// The <see cref="IFailure"/> timestamp;
        /// </summary>
        DateTime Timestamp
        { get; set; }

        /// <summary>
        /// The <see cref="IFailure"/> default value
        /// </summary>
        IFailure Default
        { get; }

        /// <summary>
        /// Clear the <see cref="IFailure"/>, 
        /// resetting it to default values
        /// </summary>
        void Clear();
    }
}
