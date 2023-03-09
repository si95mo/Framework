namespace Core.DataStructures
{
    /// <summary>
    /// Define a generic service
    /// </summary>
    public interface IService : IProperty
    { }

    /// <summary>
    /// Define a generic <see cref="IService"/> with a generic type attached
    /// </summary>
    /// <typeparam name="T">The type of service</typeparam>
    public interface IService<T> : IService where T : IProperty
    {
        /// <summary>
        /// The <see cref="Bag{T}"/> with all the <see cref="IService{T}"/> subscribers
        /// </summary>
        Bag<T> Subscribers { get; }

        /// <summary>
        /// Add an <see cref="IProperty"/> to the <see cref="IService{T}"/> subscribers
        /// </summary>
        /// <param name="item">The <see cref="IProperty"/> to add</param>
        /// <returns><see langword="true"/> if <paramref name="item"/> has been added, <see langword="false"/> otherwise</returns>
        bool Add(T item);

        /// <summary>
        /// Get an item from the <see cref="IService{T}"/> subscribers
        /// </summary>
        /// <param name="code">The code of the item to retrieve</param>
        /// <returns>The retrieved item, or <see langword="default"/> if <paramref name="code"/> has not been found</returns>
        T Get(string code);

        /// <summary>
        /// Get all the items subscribed to the <see cref="IService{T}"/>
        /// </summary>
        /// <returns>The <see cref="Bag{T}"/> with all the subscribers</returns>
        Bag<T> GetAll();
    }
}