using Extensions;
using Instructions;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Core.Scheduling
{
    [Serializable]
    public abstract class InstructionScheduler : IScheduler<IInstruction>
    {
        [field: NonSerialized()]
        protected ActionQueue<IInstruction> subscribedInstructions;

        /// <summary>
        /// The <see cref="ActionQueue{T}"/> of all the
        /// <see cref="Method"/> subscribed to the <see cref="MethodScheduler"/>
        /// </summary>
        [field: NonSerialized()]
        public ActionQueue<IInstruction> Subscribers => subscribedInstructions;

        protected ActionQueue<IInstruction> PersistentSubscribers;

        /// <summary>
        /// Initialize the parameters
        /// </summary>
        protected InstructionScheduler()
        {
            subscribedInstructions = new ActionQueue<IInstruction>();
            PersistentSubscribers = new ActionQueue<IInstruction>();
        }

        /// <summary>
        /// Add an element to the subscribed methods.
        /// </summary>
        /// <param name="instruction">The <see cref="object"/> (value) to add</param>
        public void AddElement(IInstruction instruction)
        {
            var item = SystemExtension.Clone(instruction);

            subscribedInstructions.Enqueue(item); // Add the method to the queue
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
            if (File.Exists(fileName))
            {
                Stream openFileStream = File.OpenRead(fileName);
                BinaryFormatter deserializer = new BinaryFormatter();

                ActionQueue<IInstruction> instructions = (deserializer.Deserialize(openFileStream) as InstructionScheduler)
                    .PersistentSubscribers;
                foreach (Instruction m in instructions)
                    subscribedInstructions.Enqueue(m);

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
            subscribedInstructions.Clear();
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

        public abstract IInstruction Execute();
    }
}