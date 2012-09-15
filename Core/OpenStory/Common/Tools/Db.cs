using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace OpenStory.Common.Tools
{
    /// <summary>
    /// Provides static methods for various useful database-related operations.
    /// </summary>
    public static class Db
    {
        /// <summary>
        /// A delegate that returns a database connection.
        /// </summary>
        public static Func<IDbConnection> GetConnection = GetConnectionDefault;

        /// <summary>
        /// Gets a <see cref="IDbConnection"/>.
        /// </summary>
        /// <returns>the <see cref="IDbConnection"/> instance.</returns>
        private static IDbConnection GetConnectionDefault()
        {
            throw new NotImplementedException("You need to set Db.GetConnection to use the DB helpers.");
        }

        /// <summary>
        /// Executes the provided <see cref="IDbCommand"/> and invokes a callback for the first row of the result set.
        /// </summary>
        /// <param name="command">The <see cref="IDbCommand"/> to execute.</param>
        /// <param name="callback">The Action(IDataRecord) delegate to call for the first row of the result set.</param>
        /// <returns><c>true</c> if there was a result; otherwise, <c>false</c>.</returns>
        public static bool InvokeForSingle(this IDbCommand command, Action<IDataRecord> callback)
        {
            // I actually feel quite awesome about this method, it saves me a lot of writing.
            using (var connection = GetConnection())
            {
                command.Connection = connection;

                bool result;
                connection.Open();
                using (var record = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (record.Read())
                    {
                        callback(record);
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                connection.Close();

                return result;
            }
        }

        /// <summary>
        /// Returns an iterator over the result set of the provided <see cref="IDbCommand"/>.
        /// </summary>
        /// <param name="command">The <see cref="IDbCommand"/> to execute.</param>
        /// <param name="commandBehavior">The <see cref="CommandBehavior"/> flags to pass when executing the data reader. Defaults to <see cref="CommandBehavior.Default"/>.</param>
        /// <returns>an <see cref="IEnumerable{IDataRecord}"/> for the result set of the query.</returns>
        public static IEnumerable<IDataRecord> Enumerate(this IDbCommand command,
                                                         CommandBehavior commandBehavior = CommandBehavior.Default)
        {
            using (var connection = GetConnection())
            {
                command.Connection = connection;

                connection.Open();
                using (var record = command.ExecuteReader(commandBehavior))
                {
                    while (record.Read())
                    {
                        yield return record;
                    }
                }
                connection.Close();
            }
        }

        /// <summary>
        /// Enumerates the result set of the provided <see cref="IDbCommand"/> and invokes the provided callback for each record.
        /// </summary>
        /// <param name="command">The <see cref="IDbCommand"/> to execute.</param>
        /// <param name="callback">The action to perform on each record.</param>
        /// <returns>the number of records in the result set.</returns>
        public static int InvokeForAll(this IDbCommand command, Action<IDataRecord> callback)
        {
            int count = 0;
            foreach (var record in command.Enumerate())
            {
                callback.Invoke(record);
                count++;
            }
            return count;
        }

        /// <summary>
        /// Executes the provided <see cref="IDbCommand"/> and returns the scalar result from it.
        /// </summary>
        /// <typeparam name="TResult">The type to cast the result to.</typeparam>
        /// <param name="command">The <see cref="IDbCommand"/> to execute.</param>
        /// <returns> the result from the query, cast to <typeparamref name="TResult"/>. </returns>
        public static TResult GetScalar<TResult>(this IDbCommand command)
        {
            using (var connection = GetConnection())
            {
                command.Connection = connection;

                connection.Open();
                var result = command.ExecuteScalar();
                connection.Close();

                return (TResult)result;
            }
        }

        /// <summary>
        /// Executes the provided <see cref="IDbCommand"/> as a non-query and returns the number of rows affected.
        /// </summary>
        /// <param name="command">The <see cref="IDbCommand"/> to execute as a non-query.</param>
        /// <returns>the number of rows affected by the <see cref="SqlCommand"/>.</returns>
        public static int InvokeNonQuery(this IDbCommand command)
        {
            using (var connection = GetConnection())
            {
                command.Connection = connection;

                connection.Open();
                int result = command.ExecuteNonQuery();
                connection.Close();

                return result;
            }
        }

        /// <summary>
        /// Executes the provided <see cref="IDbCommand"/> 
        /// </summary>
        /// <param name="command"></param>
        public static void InvokeStoredProcedure(this IDbCommand command)
        {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 60;

            using (var connection = GetConnection())
            {
                command.Connection = connection;

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
