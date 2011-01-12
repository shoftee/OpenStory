using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using OpenStory.Common.Tools;
using OpenStory.Server.Properties;

namespace OpenStory.Server.Data
{
    /// <summary>
    /// Provides static methods for various useful database-related operations.
    /// </summary>
    static class DbUtils
    {
        private static readonly string ConnectionString = Settings.Default.OpenStoryConnectionString;

        /// <summary>Gets a new SqlConnection with the default connection string.</summary>
        /// <returns>The SqlConnection instance.</returns>
        private static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        /// <summary>Adds a new parameter object to the SqlCommand parameters collection.</summary>
        /// <param name="sqlCommand">The SqlCommand object to add a parameter to.</param>
        /// <param name="parameterName">The parameter name string used in the SQL query.</param>
        /// <param name="dbType">The SqlDbType value corresponding to the type of the parameter.</param>
        /// <param name="value">The value to add.</param>
        public static void AddParameter(this SqlCommand sqlCommand, string parameterName, SqlDbType dbType, object value)
        {
            var parameter = new SqlParameter(parameterName, dbType) { Value = value };
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
        public static void AddParameter(this SqlCommand sqlCommand, string parameterName, SqlDbType dbType, int size,
                                        object value)
        {
            var parameter = new SqlParameter(parameterName, dbType, size) { Value = value };
            sqlCommand.Parameters.Add(parameter);
        }

        /// <summary>Executes the given query and invokes a callback for the first row of the result set.</summary>
        /// <param name="query">The SqlCommand to execute.</param>
        /// <param name="recordCallback">The Action(IDataRecord) delegate to call for the first row of the result set.</param>
        /// <returns>true if there was a result; otherwise, false.</returns>
        public static bool InvokeForSingle(SqlCommand query, Action<IDataRecord> recordCallback)
        {
            // I actually feel quite awesome about this method, it saves me a lot of writing.
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

        /// <summary>Creates a new transaction.</summary>
        /// <param name="transactionName">The name to assign to the transaction.</param>
        /// <param name="commandCallback">The callback for the work to do during this transaction.</param>
        public static void PerformTransaction(string transactionName, Action<SqlCommand> commandCallback)
        {
            PerformTransaction(transactionName, IsolationLevel.Serializable, Log.Instance, commandCallback);
        }

        /// <summary>Creates a new transaction and logs any errors to the given error logging stream.</summary>
        /// <param name="transactionName">The name to assign to the transaction.</param>
        /// <param name="errorLogger">The error logging stream to use.</param>
        /// <param name="commandCallback">The callback for the work to do during this transaction.</param>
        public static void PerformTransaction(string transactionName, ILogger errorLogger,
                                              Action<SqlCommand> commandCallback)
        {
            PerformTransaction(transactionName, IsolationLevel.Serializable, errorLogger, commandCallback);
        }

        /// <summary>Creates a new transaction with the given isolation level.</summary>
        /// <param name="transactionName">The name to assign to the transaction.</param>
        /// <param name="isolationLevel">The isolation level for the transaction.</param>
        /// <param name="commandCallback">The callback for the work to do during this transaction.</param>
        public static void PerformTransaction(string transactionName, IsolationLevel isolationLevel,
                                              Action<SqlCommand> commandCallback)
        {
            PerformTransaction(transactionName, isolationLevel, Log.Instance, commandCallback);
        }

        /// <summary>Creates a new transaction with the given isolation level and logs any errors to the given error logging stream.</summary>
        /// <param name="transactionName">The name to assign to the transaction.</param>
        /// <param name="isolationLevel">The isolation level for the transaction.</param>
        /// <param name="errorLogger">The error logging stream to use.</param>
        /// <param name="commandCallback">The callback for the work to do during this transaction.</param>
        public static void PerformTransaction(string transactionName, IsolationLevel isolationLevel, ILogger errorLogger,
                                              Action<SqlCommand> commandCallback)
        {
            using (SqlConnection connection = GetConnection())
            {
                using (SqlTransaction transaction = connection.BeginTransaction(isolationLevel, transactionName))
                {
                    using (SqlCommand command = GetTransactionCommand(connection, transaction))
                    {
                        try
                        {
                            commandCallback(command);
                            transaction.Commit();
                        }
                        catch (Exception commitException)
                        {
                            errorLogger.WriteError("Transaction {0} failed: ", commitException);
                            try
                            {
                                transaction.Rollback();
                                errorLogger.WriteInfo("Rollback successful.");
                            }
                            catch (Exception rollbackException)
                            {
                                errorLogger.WriteError("Rollback failed: ", rollbackException);
                            }
                        }
                    }
                }
            }
        }

        private static SqlCommand GetTransactionCommand(SqlConnection connection, SqlTransaction transaction)
        {
            return new SqlCommand { Connection = connection, Transaction = transaction };
        }
    }
}