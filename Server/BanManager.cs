using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using OpenMaple.Tools;

namespace OpenMaple.Server
{
    static class BanManager
    {
        private const string InsertBan = 
            "INSERT INTO Ban VALUES(BanTypeId, BanString, Reason, Expiration) " +
            "BanTypeId=@banType, BanString=@banString, Reason=@reason, Expiration=@expiration\r\n" +
            "SELECT CAST(@@IDENTITY AS INT)";

        public static readonly DateTimeOffset NoExpiration = DateTimeOffset.MaxValue;

        public static void BanByAccountId(int accountId, string reason)
        {
            BanByAccountId(accountId, reason, NoExpiration);
        }

        public static void BanByAccountId(int accountId, string reason, DateTimeOffset expiration)
        {
            SqlCommand insertBanQuery = new SqlCommand(InsertBan);

            insertBanQuery.AddParameter("@banType", SqlDbType.TinyInt, (byte) BanType.AccountId);
            insertBanQuery.AddParameter("@banString", SqlDbType.VarChar, 20, accountId.ToString());
            insertBanQuery.AddParameter("@reason", SqlDbType.VarChar, 50, reason);
            insertBanQuery.AddParameter("@expiration", SqlDbType.DateTimeOffset, expiration);

            int banId = DbUtils.GetScalar<int>(insertBanQuery);

            SqlCommand setBanQuery = new SqlCommand("UPDATE Account SET BanId = @banId WHERE AccountId=@accountId");
            setBanQuery.AddParameter("@accountId", SqlDbType.Int, accountId);
            setBanQuery.AddParameter("@banId", SqlDbType.Int, banId);

            DbUtils.ExecuteNonQuery(setBanQuery);
        }
    }

    enum BanType
    {
        None = 0,
        AccountId = 1,
        IpAddress = 2,
        MacAddress = 3,
        VolumeSerialId = 4
    }
}
