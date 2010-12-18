using System;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMaple.Tools
{
    public delegate void ReadRecordHandler(IDataRecord record);

    static class DbUtils
    {
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

        public static bool GetSingleRecord(SqlCommand query, ReadRecordHandler handler)
        {
            // I actually feel quite awesome about this method, it saves me a lot of writing.
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.OpenMapleConnectionString))
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
    }
}
