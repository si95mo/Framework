using Core.DataStructures;
using Instructions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Scheduling
{
    /// <summary>
    /// Describe a generic scheduler prototype
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IScheduler<T>
    {
        /// <summary>
        /// The subscribers
        /// </summary>
        ActionQueue<T> Instructions { get; }

        /// <summary>
        /// Add an element to the scheduler
        /// </summary>
        /// <param name="method">The element to add</param>
        void Add(T method);

        /// <summary>
        /// Remove all elements from the <see cref="IScheduler{T}"/>
        /// </summary>
        void RemoveAll();

        /// <summary>
        /// Execute the item stored
        /// </summary>
        /// <returns>The item executed</returns>
        Task<List<IInstruction>> Execute();

        /// <summary>
        /// Save the execution list
        /// </summary>
        /// <param name="fileName">The file path</param>
        void SaveExecutionList(string fileName);

        /// <summary>
        /// Load the execution list
        /// </summary>
        /// <param name="fileName">The file path</param>
        void LoadExecutionList(string fileName);
    }
}