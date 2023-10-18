using Core;
using Core.Conditions;
using Core.DataStructures;
using Core.Parameters;
using System.Threading.Tasks;

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

        /// <summary>
        /// The <see cref="IResource"/> operating state (i.e. <see cref="Status"/> is <see cref="ResourceStatus.Executing"/>)
        /// </summary>
        ICondition IsExecuting { get; }

        /// <summary>
        /// The <see cref="IResource"/> <see cref="Bag{T}"/>
        /// of <see cref="IChannel"/>
        /// </summary>
        Bag<IChannel> Channels
        { get; }

        /// <summary>
        /// The <see cref="IResource"/> status
        /// </summary>
        EnumParameter<ResourceStatus> Status
        { get; }

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
        Task Restart();
    }
}