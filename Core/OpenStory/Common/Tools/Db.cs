using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private static Func<IDbConnection> newConnection = GetConnectionDefault;

        /// <summary>
        /// Gets or sets the delegate that returns a database connection.
        /// </summary>
        public static Func<IDbConnection> NewConnection
        {
            get { return newConnection; }
            set { newConnection = value; }
        }

        /// <summary>
        /// Gets a <see cref="IDbConnection"/>.
        /// </summary>
        /// <returns>the <see cref="IDbConnection"/> instance.</returns>
        private static IDbConnection GetConnectionDefault()
        {
            throw new NotImplementedException("You need to set Db.newConnection to use the DB helpers.");
        }

        /// <summary>
        /// Executes the provided <see cref="IDbCommand"/> and invokes a callback for the first row of the result set.
        /// </summary>
        /// <param name="command">The <see cref="IDbCommand"/> to execute.</param>
        /// <param name="callback">The <see cref="Action{IDataRecord}"/> delegate to call for the first row of the result set.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="command"/> or <paramref name="callback"/> is <see langword="null"/>.</exception>
        /// <returns><see langword="true"/> if there was a result; otherwise, <see langword="false"/>.</returns>
        public static bool InvokeForSingle(this IDbCommand command, Action<IDataRecord> callback)
        {
            Guard.NotNull(() => command, command);
            Guard.NotNull(() => callback, callback);

            // I actually feel quite awesome about this method, it saves me a lot of writing.
            using (var connection = newConnection())
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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="command"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidEnumArgumentException">Thrown if <paramref name="commandBehavior"/> has an invalid value.</exception>
        /// <returns>an <see cref="IEnumerable{IDataRecord}"/> for the result set of the query.</returns>
        public static IEnumerable<IDataRecord> Enumerate(
            this IDbCommand command,
            CommandBehavior commandBehavior = CommandBehavior.Default)
        {
            Guard.NotNull(() => command, command);

            if (!Enum.IsDefined(typeof(CommandBehavior), commandBehavior))
            {
                throw new InvalidEnumArgumentException("commandBehavior", (int)commandBehavior, typeof(CommandBehavior));
            }

            using (var connection = newConnection())
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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="command"/> or <paramref name="callback"/> is <see langword="null"/>.</exception>
        /// <returns>the number of records in the result set.</returns>
        public static int InvokeForAll(this IDbCommand command, Action<IDataRecord> callback)
        {
            Guard.NotNull(() => command, command);
            Guard.NotNull(() => callback, callback);

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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="command"/> is <see langword="null"/>.</exception>
        /// <returns> the result from the query, cast to <typeparamref name="TResult"/>.</returns>
        public static TResult GetScalar<TResult>(this IDbCommand command)
        {
            Guard.NotNull(() => command, command);

            using (var connection = newConnection())
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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="command"/> is <see langword="null"/>.</exception>
        /// <returns>the number of rows affected by the <see cref="SqlCommand"/>.</returns>
        public static int InvokeNonQuery(this IDbCommand command)
        {
            Guard.NotNull(() => command, command);

            using (var connection = newConnection())
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
        /// <param name="command">The command to execute as a stored procedure.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="command"/> is <see langword="null"/>.</exception>
        public static void InvokeStoredProcedure(this IDbCommand command)
        {
            Guard.NotNull(() => command, command);

            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 60;

            using (var connection = newConnection())
            {
                command.Connection = connection;

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
