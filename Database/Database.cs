using Diagnostic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace Database
{
    /// <summary>
    /// Implement database-related functionalities
    /// </summary>
    public class Database
    {
        private SqlConnection connection;

        /// <summary>
        /// The open state of the connection
        /// </summary>
        public bool IsConnected => connection.State == System.Data.ConnectionState.Open;

        /// <summary>
        /// Create a new instance of <see cref="Database"/>
        /// </summary>
        /// <param name="connectionString">The connection <see cref="string"/></param>
        public Database(string connectionString)
        {
            connection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Create a new instance of <see cref="Database"/>
        /// </summary>
        /// <param name="connectionBuilder">The <see cref="SqlConnectionStringBuilder"/></param>
        public Database(SqlConnectionStringBuilder connectionBuilder) : this(connectionBuilder.ConnectionString)
        { }

        /// <summary>
        /// Initialize the connection to the database
        /// </summary>
        /// <returns>The (async) <see cref="Task{TResult}"/> (of which the result will be <see langword="true"/> if connected, <see langword="false"/> otherwise</returns>
        public async Task<bool> InitializeConnection()
        {
            await connection.OpenAsync();

            SqlCommand command = new SqlCommand("SELECT @@VERSION", connection);
            string version = command.ExecuteScalar().ToString();

            bool connected;
            if (IsConnected)
            {
                await Logger.InfoAsync($"Database connection initialized. Version: {version}");
                connected = true;
            }
            else
            {
                await Logger.ErrorAsync("Database connection not initialzed on Initialize(string connectionString) method!");
                connected = false;
            }

            return connected;
        }
        
        /// <summary>
        /// Close the <see cref="DatabaseManager"/> connection
        /// </summary>
        public void Close()
        {
            connection.Close();

            if (!IsConnected)
                Logger.Info("Database connection closed");
            else
                Logger.Error("Database connection not closed on Close() method!");
        }

        /// <summary>
        /// Execute a select query
        /// </summary>
        /// <param name="select">The select parameter of the query (columns name)</param>
        /// <param name="from">The from parameter of the query (table name)</param>
        /// <param name="where">The where parameter of the query (conditions)</param>
        /// <param name="other">Other parameters (e.g. "ORDER BY id DESC")</param>
        /// <returns>
        /// The <see cref="SqlDataReader"/>, in which data can be accessed
        /// with the relative column name
        /// </returns>
        /// <remarks>
        /// The <see cref="SqlDataReader"/> can be accessed, if there is a column
        /// named, for example, description, as: <br/>
        /// descriptionValue = sqlReader["description"].ToString();
        /// </remarks>
        public async Task<SqlDataReader> Select(string select, string from, string where = "", string other = "")
        {
            string query = $"SELECT {select} FROM {from}";

            if (where.CompareTo("") != 0)
                query += $" WHERE {where}";

            if (other.CompareTo("") != 0)
                query += $" {other}";

            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = await command.ExecuteReaderAsync();

            return reader;
        }

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
        /// The <paramref name="values"/> must be (an array) of tuples in the form
        /// of (<see cref="string"/>, <see cref="object"/>) = (column name, value to insert), <br/>
        /// with the column name with an '@' at the beginning (e.g. "@description")
        /// </remarks>
        public async Task<bool> InsertInto(string where, string what, params (string Name, object Value)[] values)
        {
            string valueNames = "";
            for (int i = 0; i < values.Length; i++)
            {
                valueNames += values[i].Name;

                if (i < values.Length - 1)
                    valueNames += ", ";
            }

            string query = $"INSERT INTO {where} ({what}) VALUES ({valueNames})";
            SqlCommand command = new SqlCommand(query, connection);

            foreach ((string Name, object Value) in values)
                command.Parameters.AddWithValue(Name, Value);

            int affectedRows = await command.ExecuteNonQueryAsync();

            return affectedRows > 0;
        }

        /// <summary>
        /// Execute e generic query to the <see cref="DatabaseManager"/>. <br/>
        /// See also <see cref="SqlCommand.ExecuteNonQuery"/> and
        /// <see cref="SqlCommand.ExecuteNonQueryAsync(System.Threading.CancellationToken)"/>
        /// </summary>
        /// <param name="query">The generic query</param>
        /// <returns>The number of affected rows</returns>
        /// <remarks>
        /// If the query does not strictly affect any rows, the returned value is -1
        /// </remarks>
        public async Task<int> Query(string query)
        {
            SqlCommand command = new SqlCommand(query, connection);
            int affectedRows = await command.ExecuteNonQueryAsync();

            return affectedRows;
        }

        /// <summary>
        /// Execute a generic query by reading the SQL file from disk. <br/>
        /// See also <see cref="Query(string)"/>
        /// </summary>
        /// <param name="path">The file path</param>
        /// <returns>The number of affected rows</returns>
        /// <remarks>
        /// If the query does not strictly affect any rows, the returned value is -1
        /// </remarks>
        public async Task<int> ExcuteQueryFromPath(string path)
        {
            int affectedRows = -1;

            if (File.Exists(path))
            {
                string query = File.ReadAllText(path);
                affectedRows = await Query(query);
            }

            return affectedRows;
        }

        /// <summary>
        /// Check if a database exists in the system ones
        /// </summary>
        /// <param name="databaseName">The database name to check</param>
        /// <returns><see langword="true"/> if the database exists, <see langword="false"/> otherwise</returns>
        public async Task<bool> CheckIfDatabaseExists(string databaseName)
        {
            string query = string.Format("SELECT database_id FROM sys.databases WHERE Name='{0}'", databaseName);

            SqlCommand command = new SqlCommand(query, connection);
            object result = await command.ExecuteScalarAsync();

            int affectedRows = -1;
            if (result != null)
                int.TryParse(result.ToString(), out affectedRows);

            bool exists = affectedRows > 0;

            return exists;
        }
    }
}