using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Database
{
    /// <summary>
    /// Define a generic entity class
    /// </summary>
    public abstract class Entity : IEntity
    {
        #region IEntity (abstract) implementation

        public abstract object PrimaryKey
        { get; protected set; }

        public abstract Task<bool> InsertInto();
        public abstract Task<bool> Update();

        #endregion

        #region Abstract methods

        /// <summary>
        /// Create a new instance of <see cref="Entity"/>
        /// </summary>
        /// <param name="reader">The <see cref="SqlDataReader"/></param>
        /// <returns>The new instance of <see cref="Entity"/></returns>
        public abstract Entity New(SqlDataReader reader);

        #endregion

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
            Entity tmp = New(reader);

            PrimaryKey = tmp.PrimaryKey;
        }

        #endregion
    }
}