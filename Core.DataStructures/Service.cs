using System;

namespace Core.DataStructures
{
    /// <summary>
    /// Define a generic <see cref="Service{T}"/>. See also <see cref="IService{T}"/>
    /// </summary>
    /// <typeparam name="T">The type of the item of the <see cref="Service{T}"/></typeparam>
    public abstract class Service<T> : IProperty, IService<T> where T : IProperty
    {
        #region IProperty implementation

        public string Code { get; protected set; }
        public object ValueAsObject { get; set; }
        public Type Type => GetType();

        #endregion IProperty implementation

        /// <summary>
        /// The <see cref="Bag{T}"/> with all the <see cref="Service{T}"/> subscribers
        /// </summary>
        protected Bag<T> Subscribers { get; }

        /// <summary>
        /// Initialize the <see cref="Service{T}"/> variables
        /// </summary>
        protected Service()
        {
            Code = Guid.NewGuid().ToString();
            Subscribers = new Bag<T>();
        }

        /// <summary>
        /// Initialize the <see cref="Service{T}"/> variables
        /// </summary>
        /// <param name="code">The code</param>
        protected Service(string code) : this()
        {
            Code = code;
        }

        #region IService implementation

        public bool Add(T item)
            => Subscribers.Add(item);

        public T Get(string code)
        {
            T item = Subscribers.Get(code);
            return item;
        }

        #endregion IService implementation
    }
}