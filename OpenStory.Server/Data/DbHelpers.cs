using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace OpenStory.Server.Data
{
    /// <summary>
    /// Provides static methods for various useful database-related operations.
    /// </summary>
    public static class DbHelpers
    {
        private const string ConnectionString =
            "Data Source=SHOFTMASTER;Initial Catalog=OpenStory;Persist Security Info=True;User ID=OpenStoryUser;Password=.@c#8sharp";

        /// <summary>
        /// Gets a new SqlConnection with the default connection string.
        /// </summary>
        /// <returns>the SqlConnection instance.</returns>
        private static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// Executes the provided <see cref="SqlCommand"/> and invokes a callback for the first row of the result set.
        /// </summary>
        /// <param name="query">The <see cref="SqlCommand"/> to execute.</param>
        /// <param name="callback">The Action(IDataRecord) delegate to call for the first row of the result set.</param>
        /// <returns><c>true</c> if there was a result; otherwise, <c>false</c>.</returns>
        public static bool InvokeForSingle(SqlCommand query, Action<IDataRecord> callback)
        {
            // I actually feel quite awesome about this method, it saves me a lot of writing.
            using (SqlConnection connection = GetConnection())
            {
                query.Connection = connection;
                connection.Open();

                using (SqlDataReader record = query.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (!record.Read()) return false;

                    callback(record);
                    return true;
                }
            }
        }

        /// <summary>
        /// Returns an iterator over the result set of the provided <see cref="SqlCommand"/>.
        /// </summary>
        /// <param name="query">The <see cref="SqlCommand"/> to execute.</param>
        /// <param name="commandBehavior">The <see cref="CommandBehavior"/> flags to pass when executing the data reader. Defaults to <see cref="CommandBehavior.Default"/>.</param>
        /// <returns>an <see cref="IEnumerable{IDataRecord}"/> for the result set of the query.</returns>
        public static IEnumerable<IDataRecord> Enumerate(this SqlCommand query, CommandBehavior commandBehavior = CommandBehavior.Default)
        {
            using (SqlConnection connection = GetConnection())
            {
                query.Connection = connection;
                connection.Open();

                using (SqlDataReader record = query.ExecuteReader(commandBehavior))
                {
                    while (record.Read())
                    {
                        yield return record;
                    }
                }
            }
        }

        /// <summary>
        /// Enumerates the result set of the provided <see cref="SqlCommand"/> and invokes the provided callback for each record.
        /// </summary>
        /// <param name="query">The <see cref="SqlCommand"/> to execute.</param>
        /// <param name="callback">The action to perform on each record.</param>
        /// <returns>the number of records in the result set.</returns>
        public static int InvokeForAll(SqlCommand query, Action<IDataRecord> callback)
        {
            int count = 0;
            foreach (IDataRecord record in query.Enumerate())
            {
                callback.Invoke(record);
                count++;
            }
            return count;
        }

        /// <summary>
        /// Executes the provided <see cref="SqlCommand"/> and returns the scalar result from it.
        /// </summary>
        /// <typeparam name="TResult">The type to cast the result to.</typeparam>
        /// <param name="scalarQuery">The <see cref="SqlCommand"/> to execute.</param>
        /// <returns> the result from the query, cast to <typeparamref name="TResult"/>. </returns>
        public static TResult GetScalar<TResult>(SqlCommand scalarQuery)
        {
            using (SqlConnection connection = GetConnection())
            {
                scalarQuery.Connection = connection;
                connection.Open();

                return (TResult)scalarQuery.ExecuteScalar();
            }
        }

        /// <summary>
        /// Executes the provided <see cref="SqlCommand"/> as a non-query and returns the number of rows affected.
        /// </summary>
        /// <param name="nonQuery">The <see cref="SqlCommand"/> to execute as a non-query.</param>
        /// <returns>the number of rows affected by the <see cref="SqlCommand"/>.</returns>
        public static int ExecuteNonQuery(SqlCommand nonQuery)
        {
            using (SqlConnection connection = GetConnection())
            {
                nonQuery.Connection = connection;
                connection.Open();

                return nonQuery.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes the provided <see cref="SqlCommand"/> 
        /// </summary>
        /// <param name="spCommand"></param>
        public static void ExecuteStoredProcedure(SqlCommand spCommand)
        {
            spCommand.CommandType = CommandType.StoredProcedure;
            spCommand.CommandTimeout = 60;

            using (SqlConnection connection = GetConnection())
            {
                spCommand.Connection = connection;
                connection.Open();

                spCommand.ExecuteNonQuery();
            }
        }
    }
}