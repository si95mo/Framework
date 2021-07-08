using Core.DataStructures;
using Extensions;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Core.Scheduling
{
    [Serializable]
    public abstract class MethodScheduler : IScheduler<Method>
    {
        [field: NonSerialized()]
        protected ActionQueue<Method> subscribedMethods;

        protected ActionQueue<Method> persistentSubscribers;

        /// <summary>
        /// The <see cref="ActionQueue{T}"/> of all the
        /// <see cref="Method"/> subscribed to the <see cref="MethodScheduler"/>
        /// </summary>
        [field: NonSerialized()]
        public ActionQueue<Method> Subscribers => subscribedMethods;

        protected ActionQueue<Method> PersistentSubscribers => persistentSubscribers;

        /// <summary>
        /// Initialize the parameters
        /// </summary>
        protected MethodScheduler()
        {
            subscribedMethods = new ActionQueue<Method>();
            persistentSubscribers = new ActionQueue<Method>();
        }

        /// <summary>
        /// Add an element to the subscribed methods.
        /// </summary>
        /// <param name="method">The <see cref="object"/> (value) to add</param>
        public void AddElement(Method method)
        {
            var item = method.DeepCopy();

            subscribedMethods.Enqueue(item); // Add the method to the queue
            PersistentSubscribers.Enqueue(item); // Add the method to the persistent queue
        }

        /// <summary>
        /// Load a <see cref="ActionQueue{T}"/> with
        /// a previous iteration performed by the
        /// <see cref="SimpleMethodScheduler"/>.
        /// </summary>
        /// <param name="fileName">The file name from which read the list</param>
        public void LoadExecutionList(string fileName)
        {
            subscribedMethods.Clear();
            PersistentSubscribers.Clear();

            if (File.Exists(fileName))
            {
                Stream openFileStream = File.OpenRead(fileName);
                BinaryFormatter deserializer = new BinaryFormatter();

                ActionQueue<Method> methods = (deserializer.Deserialize(openFileStream) as MethodScheduler)
                    ?.PersistentSubscribers;
                if (methods != null)
                {
                    foreach (Method m in methods)
                        subscribedMethods.Enqueue(m);
                }

                openFileStream.Close();
            }
            else
                throw new IOException("File not found!");
        }

        /// <summary>
        /// Removes all the <see cref="Method"/> subscribed in
        /// the <see cref="Subscribers"/>.
        /// </summary>
        public void RemoveAll()
        {
            subscribedMethods.Clear();
            PersistentSubscribers.Clear();
        }

        /// <summary>
        /// Save the last execution list of <see cref="Method"/>
        /// performed by the <see cref="MethodScheduler"/>.
        /// </summary>
        /// <param name="fileName">The file name in which save the list</param>
        public void SaveExecutionList(string fileName)
        {
            Stream SaveFileStream = File.Create(fileName);
            BinaryFormatter serializer = new BinaryFormatter();

            serializer.Serialize(SaveFileStream, this);

            SaveFileStream.Close();
        }

        public abstract Method Execute();
    }
}