using System;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMaple.Tools
{
    static class DbUtils
    {
        private static readonly string OpenMapleConnectionString = Properties.Settings.Default.OpenMapleConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(OpenMapleConnectionString);
        }

        /// <summary>
        /// Adds a new parameter object to the SqlCommand parameters collection.
        /// </summary>
        /// <param name="sqlCommand">The SqlCommand object to add a parameter to.</param>
        /// <param name="parameterName">The parameter name string used in the SQL query.</param>
        /// <param name="dbType">The SqlDbType value corresponding to the type of the parameter.</param>
        /// <param name="value">The value to add.</param>
        public static void AddParameter(this SqlCommand sqlCommand, string parameterName, SqlDbType dbType, object value)
        {
            SqlParameter parameter = new SqlParameter(parameterName, dbType) { Value = value };
            sqlCommand.Parameters.Add(parameter);
        }

        /// <summary>
        /// Adds a new parameter object to the SqlCommand parameters collection.
        /// This should only be used with SqlDbTypes that require a size restriction.
        /// </summary>
        /// <param name="sqlCommand">The SqlCommand object to add a parameter to.</param>
        /// <param name="parameterName">The parameter name string used in the SQL query.</param>
        /// <param name="dbType">The SqlDbType value corresponding to the type of the parameter.</param>
        /// <param name="size">The size for the value. Used with SqlDbType.</param>
        /// <param name="value">The value to add.</param>
        public static void AddParameter(this SqlCommand sqlCommand, string parameterName, SqlDbType dbType, int size, object value) 
        {
            SqlParameter parameter = new SqlParameter(parameterName, dbType, size) { Value = value };
            sqlCommand.Parameters.Add(parameter);
        }

        /// <summary>
        /// Executes the given query and calls the given handler for the first row of the result set.
        /// </summary>
        /// <param name="query">The SqlCommand to execute.</param>
        /// <param name="handler">The Action(IDataRecord) delegate to call for the first row of the result set.</param>
        /// <returns>true if there was a result; otherwise, false.</returns>
        public static bool GetSingleRecord(SqlCommand query, Action<IDataRecord> handler)
        {
            // I actually feel quite awesome about this method, it saves me a lot of writing.
            using (SqlConnection connection = GetConnection())
            {
                query.Connection = connection;
                connection.Open();
                using (SqlDataReader reader = query.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        handler(reader);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Returns an iterator for the result set of the given query.
        /// </summary>
        /// <param name="query">The SqlCommand to execute.</param>
        /// <returns>An IEnumerator for the result set of the query.</returns>
        public static IEnumerator<IDataRecord> GetRecordSetIterator(SqlCommand query)
        {
            using (SqlConnection connection = GetConnection())
            {
                query.Connection = connection;
                connection.Open();
                using (SqlDataReader reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return reader;
                    }
                }
            }
        }

        /// <summary>
        /// Executes the given query and returns the first column of the first row of the result set, casted to <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The type to cast the result to.</typeparam>
        /// <param name="scalarQuery">The SqlCommand to execute.</param>
        /// <returns>The result from the query, casted to <typeparamref name="TResult"/>.</returns>
        public static TResult GetScalar<TResult>(SqlCommand scalarQuery)
        {
            using (SqlConnection connection = GetConnection())
            {
                scalarQuery.Connection = connection;
                connection.Open();
                return (TResult) scalarQuery.ExecuteScalar();
            }
        }

        public static int ExecuteNonQuery(SqlCommand command)
        {
            using (SqlConnection connection = GetConnection())
            {
                command.Connection = connection;
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }
    }
}
