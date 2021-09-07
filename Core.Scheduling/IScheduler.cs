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

            if (null != Enqueued)
                Enqueued(this, null);
        }

        /// <summary>
        /// Dequeue an element from the collection and
        /// handle the Dequeued <see cref="EventHandler"/>.
        /// Also see <see cref="Queue{T}.Dequeue(T)"/>
        /// </summary>
        /// <param name="item">The item to dequeue</param>
        /// <returns>The item dequeued</returns>
        public new T Dequeue()
        {
            T item = base.Dequeue();

            if (null != Dequeued)
                Dequeued(this, null);

            return item;
        }
    }

    /// <summary>
    /// Describe a generic scheduler prototype
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IScheduler<T>
    {
        ActionQueue<T> Subscribers { get; }

        void AddElement(T method);

        void RemoveAll();

        T Execute();

        void SaveExecutionList(string fileName);

        void LoadExecutionList(string fileName);
    }
}