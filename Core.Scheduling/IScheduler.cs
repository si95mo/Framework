using Core.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Core.Scheduling
{
    /// <summary>
    /// Provides a method for performing a deep copy of an object.
    /// Binary Serialization is used to perform the copy.
    /// </summary>
    // Reference Article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx
    public static class SystemExtension
    {
        /// <summary>
        /// Perform a deep copy of the object via serialization.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>A deep copy of the object.</returns>
        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
                throw new ArgumentException("The type must be serializable.", nameof(source));

            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(source, null))
                return default;

            Stream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, source);
            stream.Seek(0, SeekOrigin.Begin);

            return (T)formatter.Deserialize(stream);
        }
    }

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
