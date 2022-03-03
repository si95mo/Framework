using Core;

namespace Control
{
    /// <summary>
    /// Define a basic prototype for a controller
    /// </summary>
    public interface IRegulator : IProperty
    {
        /// <summary>
        /// Start the control algorithm
        /// </summary>
        void Start();
    }
}