using System;
using System.Collections.Generic;

namespace Core.DataStructures
{
    /// <summary>
    /// A particular <see cref="Queue{T}"/> with an <see cref="Enqueued"/> and a <see cref="Dequeued"/> event handler
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
}