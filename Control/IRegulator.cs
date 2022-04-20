using Core;
using Hardware;

namespace Control
{
    /// <summary>
    /// Define a basic prototype for a controller
    /// </summary>
    public interface IRegulator : IProperty
    {
        /// <summary>
        /// The feedback <see cref="Channel{T}"/>
        /// </summary>
        Channel<double> Feedback { get; }

        /// <summary>
        /// Start the control algorithm
        /// </summary>
        void Start();
    }
}