using Diagnostic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Database
{
    /// <summary>
    /// Implement database-related functionalities
    /// </summary>
    public class DatabaseManager
    {
        private static SqlConnection connection;

        /// <summary>
        /// The open state of the connection
        /// </summary>
        public static bool IsConnectionOpen => connection.State == System.Data.ConnectionState.Open;

        /// <summary>
        /// Initialize the <see cref="DatabaseManager"/> connection
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        public static async Task Initialize(string connectionString)
        {
            connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            SqlCommand command = new SqlCommand("SELECT @@VERSION", connection);
            string version = command.ExecuteScalar().ToString();

            Logger.Log($"Database connection initialized. Version: {version}", Severity.Info);
        }

        /// <summary>
        /// Close the <see cref="DatabaseManager"/> connection
        /// </summary>
        public static void Close() => connection.Close();

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
        public static async Task<SqlDataReader> Select(string select, string from, string where = "", string other = "")
        {
            string query = $"SELECT {select} FROM {from}";

            if (where.CompareTo("") == 0)
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
        public static async Task<bool> InsertInto(string where, string what, params (string Name, object Value)[] values)
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

            foreach ((string Name, object Value) v in values)
                command.Parameters.AddWithValue(v.Name, v.Value);

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
        public static async Task<int> Query(string query)
        {
            SqlCommand command = new SqlCommand(query, connection);
            int affectedRows = await command.ExecuteNonQueryAsync();

            return affectedRows;
        }
    }
}