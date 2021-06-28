using Core.DataStructures;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Core.Scheduling
{
    [Serializable]
    public abstract class Scheduler : IScheduler
    {
        [field: NonSerialized()]
        protected MethodQueue<Method> subscribedMethods;

        /// <summary>
        /// The <see cref="MethodQueue{T}"/> of all the
        /// <see cref="Method"/> subscribed to the <see cref="SimpleScheduler"/>
        /// </summary>
        [field: NonSerialized()]
        public MethodQueue<Method> SubscribedMethods => subscribedMethods;

        protected MethodQueue<Method> PersistentSubscribedMethods;

        /// <summary>
        /// Initialize the parameters
        /// </summary>
        protected Scheduler()
        {
            subscribedMethods = new MethodQueue<Method>();
            PersistentSubscribedMethods = new MethodQueue<Method>();
        }

        /// <summary>
        /// Add an element to the subscribed methods.
        /// </summary>
        /// <param name="method">The <see cref="object"/> (value) to add</param>
        public void AddElement(Method method)
        {
            var item = SystemExtension.Clone(method);

            subscribedMethods.Enqueue(item); // Add the method to the queue
            PersistentSubscribedMethods.Enqueue(item); // Add the method to the persistent queue
        }

        /// <summary>
        /// Load a <see cref="MethodQueue{T}"/> with 
        /// a previous iteration performed by the
        /// <see cref="SimpleScheduler"/>.
        /// </summary>
        /// <param name="fileName">The file name from which read the list</param>
        public void LoadExecutionList(string fileName)
        {
            if (File.Exists(fileName))
            {
                Stream openFileStream = File.OpenRead(fileName);
                BinaryFormatter deserializer = new BinaryFormatter();

                subscribedMethods = (deserializer.Deserialize(openFileStream) as SimpleScheduler)
                    .PersistentSubscribedMethods;

                openFileStream.Close();
            }
            else
                throw new IOException("File not found!");
        }

        /// <summary>
        /// Removes all the <see cref="Method"/> subscribed in
        /// the <see cref="SubscribedMethods"/>.
        /// </summary>
        public void RemoveAll()
        {
            subscribedMethods.Clear();
            PersistentSubscribedMethods.Clear();
        }

        /// <summary>
        /// Save the last execution list of <see cref="Method"/>
        /// performed by the <see cref="SimpleScheduler"/>.
        /// </summary>
        /// <param name="fileName">The file name in which save the list</param>
        public void SaveExecutionList(string fileName)
        {
            Stream SaveFileStream = File.Create(fileName);
            BinaryFormatter serializer = new BinaryFormatter();

            serializer.Serialize(SaveFileStream, this);

            SaveFileStream.Close();
        }

        public abstract Method ExecuteAction();
    }
}
