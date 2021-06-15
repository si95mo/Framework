using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        STOPPED,
        /// <summary>
        /// The <see cref="IResource"/> is starting
        /// </summary>
        STARTING,
        /// <summary>
        /// The <see cref="IResource"/> is executing
        /// </summary>
        EXECUTING,
        /// <summary>
        /// The <see cref="IResource"/> is stopping
        /// </summary>
        STOPPING,
        /// <summary>
        /// The <see cref="IResource"/> is in failure
        /// </summary>
        FAILURE
    }

    public interface IResource
    {
        /// <summary>
        /// The <see cref="IResource"/> code
        /// </summary>
        string Code
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
