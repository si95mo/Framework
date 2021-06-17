using Hardware;
using System;
using System.Collections.Generic;

namespace Core.DataStructures
{
    /// <summary>
    /// Implements a service broker
    /// </summary>
    public class ServiceBroker
    {
        private static Bag<IResource> resources;

        /// <summary>
        /// The set of all the <see cref="IResource"/> subscribed
        /// to the <see cref="ServiceBroker"/>
        /// </summary>
        public static Bag<IResource> Resources => resources;

        /// <summary>
        /// Initialize the <see cref="ServiceBroker"/>
        /// </summary>
        public static void Init()
        {
            resources = new Bag<IResource>();
        }

        /// <summary>
        /// Add an <see cref="IResource"/> to 
        /// the <see cref="ServiceBroker"/>
        /// </summary>
        /// <param name="resource">The <see cref="IResource"/></param>
        public static void Add(IResource resource)
        {
            if (!resources.ContainsKey(resource.Code))
                resources.Add(resource);
            else
                throw new ArgumentException("The given key was already in the collection!");
        }

        /// <summary>
        /// Get an <see cref="IResource"/> from the collection.
        /// </summary>
        /// <typeparam name="IResource">The type of the item to retrieve</typeparam>
        /// <param name="code">The <see cref="IResource"/> code to fetch</param>
        /// <returns>The <see cref="IResource"/> if present in the collection, <see langword="null"/> otherwise</returns>
        public static IResource Get<IResource>(string code)
        {
            IProperty resource = null;
            resources.TryGetValue(code, out resource);

            return (IResource)resource;
        }
    }
}
