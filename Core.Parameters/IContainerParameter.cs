using Core.DataStructures;

namespace Core.Parameters
{
    /// <summary>
    /// Define a container parameter interface
    /// </summary>
    public interface IContainerParameter : IParameter
    {
        /// <summary>
        /// The <see cref="Bag{T}"/> of <see cref="IParameter"/>
        /// </summary>
        Bag<IParameter> SubParameters { get; }
    }
}