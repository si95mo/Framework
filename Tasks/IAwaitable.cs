using Core.DataStructures;
using Core.Parameters;
using System.Threading.Tasks;

namespace Tasks
{
    /// <summary>
    /// Define a generic awaitable task
    /// </summary>
    public interface IAwaitable
    {
        /// <summary>
        /// The <see cref="IAwaitable"/> <see cref="TaskStatus"/>
        /// </summary>
        EnumParameter<TaskStatus> Status { get; }

        /// <summary>
        /// The actual <see cref="IAwaitable"/> message (i.e. last executed operation)
        /// </summary>
        StringParameter Message { get; }

        /// <summary>
        /// The <see cref="IAwaitable"/> input <see cref="IParameter"/>
        /// </summary>
        Bag<IParameter> InputParameters { get; }

        /// <summary>
        /// The <see cref="IAwaitable"/> output <see cref="IParameter"/>
        /// </summary>
        Bag<IParameter> OutputParameters { get; }

        /// <summary>
        /// Start the <see cref="IAwaitable"/> task
        /// </summary>
        /// <returns>The actual <see cref="Task"/> associated with the <see cref="IAwaitable"/></returns>
        Task Start();
    }
}
