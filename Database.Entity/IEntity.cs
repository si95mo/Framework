using System.Data.SqlClient;
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

    /// <summary>
    /// Define a factory pattern to create <see cref="IEntity"/> objects
    /// </summary>
    internal interface IEntityFactory
    {
        /// <summary>
        /// Create a new <see cref="IEntity"/> object
        /// </summary>
        /// <remarks><see cref="SqlDataReader.Read"/> must be called inside the method!</remarks>
        /// <param name="reader">The <see cref="SqlDataReader"/></param>
        /// <returns>The new <see cref="IEntity"/> object created</returns>
        IEntity New(SqlDataReader reader);
    }
}