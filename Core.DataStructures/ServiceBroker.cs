using System.Collections;
using System.Collections.Generic;

namespace Core.DataStructures
{
    /// <summary>
    /// Implements a service broker
    /// </summary>
    public class ServiceBroker : IEnumerable<IProperty>
    {
        /// <summary>
        /// <see langword="true"/> if the <see cref="ServiceBroker"/> has been initialized, <see langword="false"/> otherwise
        /// </summary>
        public static bool Initialized { get; private set; } = false;

        private static Bag<IProperty> subscribers;

        /// <summary>
        /// Initialize the <see cref="ServiceBroker"/>
        /// </summary>
        public static void Initialize()
        {
            subscribers = new Bag<IProperty>();
            Initialized = true;
        }

        /// <summary>
        /// Clear the <see cref="ServiceBroker"/> collection
        /// </summary>
        public static void Clear()
            => Initialize();

        /// <summary>
        /// Add an item to the <see cref="ServiceBroker"/>
        /// </summary>
        /// <typeparam name="T">The type of the item</typeparam>
        /// <param name="item">The item to add</param>
        /// <returns><see langword="true"/> if the item is added,
        /// <see langword="false"/> otherwise</returns>
        public static bool Add<T>(IProperty item)
            => subscribers.Add(item);

        /// <summary>
        /// Get the collection relative to the specified type
        /// </summary>
        /// <typeparam name="T">The type of the collection to return</typeparam>
        /// <returns>The <see cref="Bag{T}"/> containing the item retrieved
        /// from the <see cref="ServiceBroker"/></returns>
        public static Bag<T> Get<T>() where T : class
        {
            Bag<T> returnCollection = new Bag<T>();

            List<IProperty> sublist = subscribers.ToList();
            foreach (IProperty item in sublist)
            {
                if (item is T)
                    returnCollection.Add(item);
            }

            return returnCollection;
        }

        /// <summary>
        /// Get the <see cref="IEnumerator{T}"/> used to iterate through the <see cref="ServiceBroker"/>
        /// </summary>
        /// <returns>The <see cref="IEnumerator{T}"/></returns>
        public IEnumerator<IProperty> GetEnumerator() => subscribers.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}