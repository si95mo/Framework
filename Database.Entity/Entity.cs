using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Database.Entity
{
    /// <summary>
    /// Define a generic entity class
    /// </summary>
    public abstract class Entity : IEntity, IEntityFactory
    {
        #region IEntity (abstract) implementation

        public abstract object PrimaryKey
        { get; protected set; }

        public abstract string TableName
        { get; protected set; }

        public abstract Task<bool> InsertInto();

        public abstract Task<bool> Update();

        #endregion IEntity (abstract) implementation

        #region IEntityFactory (abstract) implementation

        public abstract IEntity New(SqlDataReader reader);

        #endregion IEntityFactory (abstract) implementation

        #region Protected methods

        /// <summary>
        /// Update the instance <see cref="PrimaryKey"/> in case of an
        /// <see cref="InsertInto"/> with an auto-incremented primary key in the database
        /// </summary>
        /// <param name="from">The table name</param>
        /// <returns>The (async) <see cref="Task"/></returns>
        protected async Task UpdatePrimaryKey(string from)
        {
            string select = "TOP(1) *";
            string other = "ORDER BY 1 DESC";

            SqlDataReader reader = await DatabaseManager.Select(select, from, other: other);
            reader.Read(); // Only one record
            IEntity tmp = New(reader);

            PrimaryKey = tmp.PrimaryKey;
        }

        #endregion Protected methods
    }
}