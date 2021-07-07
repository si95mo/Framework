using System.Collections;
using System.Collections.Generic;

namespace Core.DataStructures
{
    /// <summary>
    /// Implements a service broker
    /// </summary>
    public class ServiceBroker : IEnumerable<string>
    {
        private static Bag<IProperty> subscribers;

        /// <summary>
        /// Initialize the <see cref="ServiceBroker"/>
        /// </summary>
        public static void Init()
        {
            subscribers = new Bag<IProperty>();
        }

        /// <summary>
        /// Clear the <see cref="ServiceBroker"/> collection.
        /// </summary>
        public static void Clear() => Init();

        /// <summary>
        /// Add an item to the <see cref="ServiceBroker"/>
        /// </summary>
        /// <typeparam name="T">The type of the item</typeparam>
        /// <param name="item">The item to add</param>
        /// <returns><see langword="true"/> if the item is added,
        /// <see langword="false"/> otherwise</returns>
        public static bool Add<T>(IProperty item) => subscribers.Add(item);

        /// <summary>
        /// Get the collection relative to the specified type
        /// </summary>
        /// <typeparam name="T">The type of the collection to return</typeparam>
        /// <returns>The <see cref="Bag{T}"/> containing the item retrieved
        /// from the <see cref="ServiceBroker"/></returns>
        public static Bag<T> Get<T>() where T : class
        {
            Bag<T> returnCollection = new Bag<T>();

            var sublist = subscribers.ToList();
            foreach (var item in sublist)
            {
                if (item is T)
                    returnCollection.Add(item);
            }

            return returnCollection;
        }

        public IEnumerator<string> GetEnumerator() => subscribers.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}