using Core;
using Core.DataStructures;

namespace Hardware
{
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

        Bag<IChannel> Channels
        { get; }

        /// <summary>
        /// The <see cref="IResource"/> status
        /// </summary>
        ResourceStatus Status
        { get; }

        /// <summary>
        /// The last <see cref="IFailure"/>
        /// </summary>
        IFailure LastFailure
        { get; }

        /// <summary>
        /// Start the <see cref="IResource"/>
        /// </summary>
        void Start();

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