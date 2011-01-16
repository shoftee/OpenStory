using System;
using System.Data;
using OpenStory.Common;

namespace OpenStory.Server.Data
{
    internal class BanEngine
    {
        private const string InsertBanSql =
            "INSERT INTO Ban VALUES(BanTypeId, BanString, Reason, Expiration) " +
            "BanTypeId=@banType, BanString=@banString, Reason=@reason, Expiration=@expiration\r\n" +
            "SELECT CAST(@@IDENTITY AS INT)";

        private const string SetBanIdSql =
            "UPDATE Account SET BanId=@banId " +
            "WHERE AccountId=@accountId";

        public static readonly DateTimeOffset NoExpiration = DateTimeOffset.MaxValue;

        public static void BanByAccountId(int accountId, string reason)
        {
            BanByAccountId(accountId, reason, NoExpiration);
        }

        public static void BanByAccountId(int accountId, string reason, DateTimeOffset expiration)
        {
            DbUtils.PerformTransaction("BanByAccountId", command =>
            {
                command.CommandText = InsertBanSql;
                command.AddParameter("@banType", SqlDbType.TinyInt, (byte) BanType.AccountId);
                command.AddParameter("@banString", SqlDbType.VarChar, 20, accountId.ToString());
                command.AddParameter("@reason", SqlDbType.VarChar, 50, reason);
                command.AddParameter("@expiration", SqlDbType.DateTimeOffset, expiration);
                var banId = (int) command.ExecuteScalar();

                command.Parameters.Clear();
                command.CommandText = SetBanIdSql;
                command.AddParameter("@accountId", SqlDbType.Int, accountId);
                command.AddParameter("@banId", SqlDbType.Int, banId);
                command.ExecuteNonQuery();
            });
        }
    }
}