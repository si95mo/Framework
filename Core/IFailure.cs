using Microsoft.SqlServer.Server;
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
        /// The (eventual) <see cref="System.Exception"/> associated to the <see cref="IFailure"/>
        /// </summary>
        Exception Exception { get; set; }

        /// <summary>
        /// The <see cref="IFailure"/> default value
        /// </summary>
        IFailure Default
        { get; }

        /// <summary>
        /// Notify when the <see cref="IFailure"/> changed
        /// </summary>
        /// <remarks>The property that will trigger the <see langword="event"/> is <see cref="Timestamp"/></remarks>
        event EventHandler<ValueChangedEventArgs> FailureChanged;

        /// <summary>
        /// Clear the <see cref="IFailure"/>, resetting it to default values
        /// </summary>
        void Clear();

        /// <summary>
        /// Update the <see cref="IFailure"/>
        /// </summary>
        /// <param name="description">The description</param>
        /// <param name="ex">The (eventual) <see cref="Exception"/></param>
        void Update(string description, Exception ex = default);
    }
}