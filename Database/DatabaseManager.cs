using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Database
{
    /// <summary>
    /// Implement database-related functionalities
    /// </summary>
    /// <remarks>This class only works with one database connection at time. For multiple (and simultaneous connections see <see cref="Database"/>)</remarks>
    public class DatabaseManager
    {
        private static Database database;

        /// <summary>
        /// The open state of the connection
        /// </summary>
        public static bool IsConnected => database.IsConnected;

        /// <summary>
        /// Initialize the <see cref="DatabaseManager"/> connection
        /// </summary>
        /// <param name="connectionString">The connection <see cref="string"/></param>
        /// <returns>The (async) <see cref="Task{TResult}"/> (of which the result will be <see langword="true"/> if connected, <see langword="false"/> otherwise</returns>
        public static async Task<bool> Initialize(string connectionString)
        {
            database = new Database(connectionString);

            bool connected = await database.InitializeConnection();
            return connected;
        }

        /// <summary>
        /// Initialize the <see cref="DatabaseManager"/> connection
        /// </summary>
        /// <param name="sqlConnectionBuilder">The <see cref="SqlConnectionStringBuilder"/></param>
        /// <returns>The (async) <see cref="Task{TResult}"/> (of which the result will be <see langword="true"/> if connected, <see langword="false"/> otherwise</returns>
        public static async Task<bool> Initialize(SqlConnectionStringBuilder sqlConnectionBuilder)
            => await Initialize(sqlConnectionBuilder.ConnectionString);

        /// <summary>
        /// Close the <see cref="DatabaseManager"/> connection
        /// </summary>
        public static void Close()
            => database.Close();

        /// <summary>
        /// Execute a select query
        /// </summary>
        /// <param name="select">The select parameter of the query (columns name)</param>
        /// <param name="from">The from parameter of the query (table name)</param>
        /// <param name="where">The where parameter of the query (conditions)</param>
        /// <param name="other">Other parameters (e.g. "ORDER BY id DESC")</param>
        /// <returns>
        /// The <see cref="SqlDataReader"/>, in which data can be accessed with the relative column name
        /// </returns>
        /// <remarks>
        /// The <see cref="SqlDataReader"/> can be accessed, if there is a column named, for example, description, as: descriptionValue = sqlReader["description"].ToString();
        /// </remarks>
        public static async Task<SqlDataReader> Select(string select, string from, string where = "", string other = "")
            => await database.Select(select, from, where, other);

        /// <summary>
        /// Execute an insert into query
        /// </summary>
        /// <param name="where">Where insert (the table name)</param>
        /// <param name="what">What insert (the columns name)</param>
        /// <param name="values">The values to insert</param>
        /// <returns>
        /// <see langword="true"/> if the query inserted at least a row,
        /// <see langword="false"/> otherwise
        /// </returns>
        /// <remarks>
        /// The <paramref name="values"/> must be (an array) of tuples in the form of (<see cref="string"/>, <see cref="object"/>) = (column name, value to insert),
        /// with the column name with an '@' at the beginning (e.g. "@description")
        /// </remarks>
        public static async Task<bool> InsertInto(string where, string what, params (string Name, object Value)[] values)
            => await database.InsertInto(where, what, values);

        /// <summary>
        /// Execute e generic query to the <see cref="DatabaseManager"/>. <br/>
        /// See also <see cref="SqlCommand.ExecuteNonQuery"/> and <see cref="SqlCommand.ExecuteNonQueryAsync(System.Threading.CancellationToken)"/>
        /// </summary>
        /// <param name="query">The generic query</param>
        /// <returns>The number of affected rows</returns>
        /// <remarks>
        /// If the query does not strictly affect any rows, the returned value is -1
        /// </remarks>
        public static async Task<int> Query(string query)
            => await database.Query(query);

        /// <summary>
        /// Execute a generic query by reading the SQL file from disk. <br/>
        /// See also <see cref="Query(string)"/>
        /// </summary>
        /// <param name="path">The file path</param>
        /// <returns>The number of affected rows</returns>
        /// <remarks>
        /// If the query does not strictly affect any rows, the returned value is -1
        /// </remarks>
        public static async Task<int> ExcuteQueryFromPath(string path)
            => await database.ExcuteQueryFromPath(path);

        /// <summary>
        /// Check if a database exists in the system ones
        /// </summary>
        /// <param name="databaseName">The database name to check</param>
        /// <returns><see langword="true"/> if the database exists, <see langword="false"/> otherwise</returns>
        public static async Task<bool> CheckIfDatabaseExists(string databaseName)
            => await database.CheckIfDatabaseExists(databaseName);
    }
}