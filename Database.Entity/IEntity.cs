using System.Threading.Tasks;

namespace Database.Entity
{
    /// <summary>
    /// Define an entity class prototype
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// The <see cref="IEntity"/> primary key
        /// </summary>
        object PrimaryKey { get; }

        /// <summary>
        /// The <see cref="IEntity"/> associated table name inside the database
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// Update the the database record with the <see cref="IEntity"/> class actual value
        /// </summary>
        /// <returns>The (async) <see cref="Task{TResult}"/> (<see langword="true"/> if update succeeded, <see langword="false"/> otherwise</returns>
        Task<bool> Update();

        /// <summary>
        /// Insert a new record in the database
        /// </summary>
        /// <returns>The (async) <see cref="Task{TResult}"/> (<see langword="true"/> if insert succeeded, <see langword="false"/> otherwise</returns>
        Task<bool> InsertInto();
    }
}