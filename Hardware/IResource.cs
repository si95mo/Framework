using Core;
using Core.DataStructures;
using System;
using System.Threading.Tasks;

namespace Hardware
{
    /// <summary>
    /// Handles the property status changed event.
    /// See also <see cref="EventArgs"/>
    /// </summary>
    public class StatusChangedEventArgs : EventArgs
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
        /// Create a new instance of <see cref="StatusChangedEventArgs"/>
        /// </summary>
        /// <param name="oldValue">The old value</param>
        /// <param name="newValue">The new value</param>
        public StatusChangedEventArgs(object oldValue, object newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    /// <summary>
    /// Define the <see cref="IResource"/> status
    /// </summary>
    public enum ResourceStatus
    {
        /// <summary>
        /// The <see cref="IResource"/> is stopped
        /// </summary>
        Stopped = 0,

        /// <summary>
        /// The <see cref="IResource"/> is starting
        /// </summary>
        Starting = 1,

        /// <summary>
        /// The <see cref="IResource"/> is executing
        /// </summary>
        Executing = 2,

        /// <summary>
        /// The <see cref="IResource"/> is stopping
        /// </summary>
        Stopping = 3,

        /// <summary>
        /// The <see cref="IResource"/> is in failure
        /// </summary>
        Failure = 5
    }

    /// <summary>
    /// Describe a generic hardware resource
    /// </summary>
    public interface IResource : IProperty
    {
        /// <summary>
        /// The <see cref="IResource"/> operating state
        /// </summary>
        bool IsOpen
        { get; }

        /// <summary>
        /// The <see cref="IResource"/> <see cref="Bag{T}"/>
        /// of <see cref="IChannel"/>
        /// </summary>
        Bag<IChannel> Channels
        { get; }

        /// <summary>
        /// The <see cref="IResource"/> status
        /// </summary>
        ResourceStatus Status
        { get; }

        /// <summary>
        /// The <see cref="IResource"/> <see cref="Status"/> value
        /// changed event listener
        /// </summary>
        event EventHandler<StatusChangedEventArgs> StatusChanged;

        /// <summary>
        /// The last <see cref="IFailure"/>
        /// </summary>
        IFailure LastFailure
        { get; }

        /// <summary>
        /// Start the <see cref="IResource"/>
        /// </summary>
        Task Start();

        /// <summary>
        /// Stop the <see cref="IResource"/>
        /// </summary>
        void Stop();

        /// <summary>
        /// Restart the <see cref="IResource"/>
        /// </summary>
        void Restart();
    }
}