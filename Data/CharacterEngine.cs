using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using OpenMaple.Tools;

namespace OpenMaple.Data
{
    static class CharacterEngine
    {
        public static bool IsNameAvailable(string name)
        {
            SqlCommand query = new SqlCommand("SELECT COUNT(*) FROM [dbo].[Character] WHERE [Name]=@name");
            query.AddParameter("@name", SqlDbType.VarChar, 12, name);

            int count = DbUtils.GetScalar<int>(query);
            return (count == 0);
        }
    }
}
