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
        private static Bag<IService> services;

        /// <summary>
        /// Initialize the <see cref="ServiceBroker"/>
        /// </summary>
        public static void Initialize()
        {
            subscribers = new Bag<IProperty>();
            services = new Bag<IService>();

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
        /// Check if the <see cref="ServiceBroker"/> can provide an <see cref="IService{T}"/> (i.e. the service has already been added
        /// </summary>
        /// <param name="code">The code of service to check</param>
        /// <returns><see langword="true"/> if the service can be provided, <see langword="false"/> otherwise</returns>
        public bool CanProvide(string code)
        {
            bool canProvide = services.ContainsKey(code);
            if (canProvide)
            {
                IService service = services.Get(code);
                canProvide = service.GetType().IsAssignableFrom(services.Get(service.Code).GetType());
            }

            return canProvide;
        }

        /// <summary>
        /// Check if the <see cref="ServiceBroker"/> can provide an <see cref="IService{T}"/>
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IService{T}"/> to check</typeparam>
        /// <returns><see langword="true"/> if the service can be provided, <see langword="false"/> otherwise</returns>
        public bool CanProvide<T>() where T : IService
        {
            bool canProvide = true;
            foreach(IService service in services)
                canProvide &= typeof(T).IsAssignableFrom(service.GetType());

            return canProvide;
        }

        /// <summary>
        /// Provide an <see cref="IService"/>
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IService"/> to provide</typeparam>
        /// <param name="service">The <see cref="IService"/> to provide</param>
        public void Provide<T>(T service) where T : IService
        {
            string code = service.Code;
            if(!CanProvide(code)) // The service has not been added yet
                services.Add(service);
        }

        /// <summary>
        /// Get the <see cref="IEnumerator{T}"/> used to iterate through the <see cref="ServiceBroker"/>
        /// </summary>
        /// <returns>The <see cref="IEnumerator{T}"/></returns>
        public IEnumerator<IProperty> GetEnumerator() => subscribers.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}