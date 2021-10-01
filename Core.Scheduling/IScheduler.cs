using System;
using System.Collections.Generic;

namespace Core.Scheduling
{
    /// <summary>
    /// An <see cref="ActionQueue{T}"/> with <see cref="Enqueued"/> and
    /// <see cref="Dequeued"/> event handlers.
    /// See <see cref="Queue{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Queue{T}"/> items</typeparam>
    [Serializable]
    public class ActionQueue<T> : Queue<T>
    {
        /// <summary>
        /// The <see cref="Enqueue(T)"/> <see cref="EventHandler"/>
        /// </summary>
        public event EventHandler Enqueued;

        /// <summary>
        /// The <see cref="Dequeue"/> <see cref="EventHandler"/>
        /// </summary>
        public event EventHandler Dequeued;

        /// <summary>
        /// Enqueue an element to the collection and
        /// handle the Enqueued <see cref="EventHandler"/>.
        /// Also see <see cref="Queue{T}.Enqueue(T)"/>
        /// </summary>
        /// <param name="item">The item to enqueue</param>
        public new void Enqueue(T item)
        {
            base.Enqueue(item);
            Enqueued?.Invoke(this, null);
        }

        /// <summary>
        /// Dequeue an element from the collection and
        /// handle the Dequeued <see cref="EventHandler"/>.
        /// Also see <see cref="Queue{T}.Dequeue()"/>
        /// </summary>
        /// <returns>The item dequeued</returns>
        public new T Dequeue()
        {
            T item = base.Dequeue();
            Dequeued?.Invoke(this, null);

            return item;
        }
    }

    /// <summary>
    /// Describe a generic scheduler prototype
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IScheduler<T>
    {
        /// <summary>
        /// The subscribers
        /// </summary>
        ActionQueue<T> Subscribers { get; }

        /// <summary>
        /// Add an element to the scheduler
        /// </summary>
        /// <param name="method">The element to add</param>
        void AddElement(T method);

        /// <summary>
        /// Remove all elements from the <see cref="IScheduler{T}"/>
        /// </summary>
        void RemoveAll();

        /// <summary>
        /// Execute the item stored
        /// </summary>
        /// <returns>The item executed</returns>
        T Execute();

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