using Core.DataStructures;
using System;
using System.Collections.Generic;

namespace Core.Scheduling
{
    /// <summary>
    /// A <see cref="MethodQueue{T}"/> with Equeued and Dequeued event handler.
    /// See <see cref="Queue{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the list items</typeparam>
    [Serializable]
    public class MethodQueue<T> : Queue<T>
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

    public interface IScheduler
    {
        MethodQueue<Method> SubscribedMethods { get; }

        void AddElement(Method method);

        void RemoveAll();

        Method ExecuteAction();

        void SaveExecutionList(string fileName);

        void LoadExecutionList(string fileName);
    }
}