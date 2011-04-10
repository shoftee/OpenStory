using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace OpenStory.Server.Data
{
    /// <summary>
    /// Provides static methods for various useful database-related operations.
    /// </summary>
    static class DbHelpers
    {
        private const string ConnectionString =
            "Data Source=SHOFTBOX;Initial Catalog=OpenStory;Persist Security Info=True;User ID=OpenStoryUser;Password=.@c#8sharp";

        /// <summary>Gets a new SqlConnection with the default connection string.</summary>
        /// <returns>The SqlConnection instance.</returns>
        private static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        /// <summary>Executes the given query and invokes a callback for the first row of the result set.</summary>
        /// <param name="query">The SqlCommand to execute.</param>
        /// <param name="recordCallback">The Action(IDataRecord) delegate to call for the first row of the result set.</param>
        /// <returns>true if there was a result; otherwise, false.</returns>
        public static bool InvokeForSingle(SqlCommand query, Action<IDataRecord> recordCallback)
        {
            // I actually feel quite awesome about this method, it saves me a lot of writing.
            using (query)
            using (SqlConnection connection = GetConnection())
            {
                query.Connection = connection;
                connection.Open();
                using (SqlDataReader record = query.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (!record.Read()) return false;

                    recordCallback(record);
                    return true;
                }
            }
        }

        /// <summary>Returns an iterator for the result set of the given query.</summary>
        /// <param name="query">The SqlCommand to execute.</param>
        /// <returns>An IEnumerator for the result set of the query.</returns>
        public static IEnumerable<IDataRecord> GetRecordSetIterator(SqlCommand query)
        {
            using (query)
            using (SqlConnection connection = GetConnection())
            {
                query.Connection = connection;
                connection.Open();
                using (SqlDataReader record = query.ExecuteReader())
                {
                    while (record.Read())
                    {
                        yield return record;
                    }
                }
            }
        }

        /// <summary>Iterates over the results of a SqlCommand and invokes a callback for each record.</summary>
        /// <param name="query">The SqlCommand to execute.</param>
        /// <param name="recordCallback">The action to perform on each record.</param>
        /// <returns>The number of records in the result set.</returns>
        public static int InvokeForAll(SqlCommand query, Action<IDataRecord> recordCallback)
        {
            int count = 0;
            foreach (IDataRecord record in GetRecordSetIterator(query))
            {
                recordCallback.Invoke(record);
                count++;
            }
            return count;
        }

        /// <summary>Executes the given query and returns the first column of the first row of the result set.</summary>
        /// <typeparam name="TResult">The type to cast the result to.</typeparam>
        /// <param name="scalarQuery">The SqlCommand to execute.</param>
        /// <returns>The result from the query, casted to <typeparamref name="TResult"/>.</returns>
        public static TResult GetScalar<TResult>(SqlCommand scalarQuery)
        {
            using (scalarQuery)
            using (SqlConnection connection = GetConnection())
            {
                scalarQuery.Connection = connection;
                connection.Open();
                return (TResult) scalarQuery.ExecuteScalar();
            }
        }

        /// <summary>Executes the given SqlCommand as a non-query and returns the number of rows affected.</summary>
        /// <param name="nonQuery">The SqlCommand to execute as a non-query.</param>
        /// <returns>The number of rows affected by the SqlCommand.</returns>
        public static int ExecuteNonQuery(SqlCommand nonQuery)
        {
            using (SqlConnection connection = GetConnection())
            {
                nonQuery.Connection = connection;
                connection.Open();
                return nonQuery.ExecuteNonQuery();
            }
        }

        public static void ExecuteStoredProcedure(SqlCommand spCommand)
        {
            spCommand.CommandType = CommandType.StoredProcedure;
            spCommand.CommandTimeout = 60;
            using (SqlConnection connection = GetConnection())
            {
                spCommand.Connection = connection;
                spCommand.ExecuteNonQuery();
            }
        }
    }
}