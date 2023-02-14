using Core.DataStructures;
using System;
using System.Collections.Generic;

namespace Core.Parameters
{
    /// <summary>
    /// Define an abstract container parameter
    /// </summary>
    /// <typeparam name="T">The type of the contained objects</typeparam>
    public abstract class ContainerParameter<T> : Parameter<T>, IContainerParameter where T : IParameter
    {
        /// <summary>
        /// The <see cref="Bag{T}"/> of sub parameters
        /// </summary>
        public Bag<T> SubParameters { get; protected set; }

        Bag<IParameter> IContainerParameter.SubParameters => (Bag<IParameter>)Convert.ChangeType(SubParameters, typeof(Bag<IParameter>));

        /// <summary>
        /// The <see cref="SubParameters"/> as a <see cref="List{T}"/>
        /// </summary>
        protected List<T> SubParametersAsList => (List<T>)Convert.ChangeType(SubParameters.ToList(), typeof(T));

        /// <summary>
        /// Initialize <see cref="ContainerParameter{T}"/> attributes
        /// </summary>
        /// <param name="code">The code</param>
        protected ContainerParameter(string code) : base(code)
        {
            SubParameters = new Bag<T>();
        }

        /// <summary>
        /// Add an item to the <see cref="IContainerParameter"/>
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
            => SubParameters.Add(item);

        /// <summary>
        /// Get an element from the <see cref="ContainerParameter{T}"/>
        /// </summary>
        /// <param name="code">The code of the element to get</param>
        /// <returns>The retrieved element, or <see langword="default"/> if nothing was found</returns>
        public IParameter Get(string code)
            => SubParameters.Get(code);
    }
}