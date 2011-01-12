namespace OpenStory.Server.Data
{
    internal static class GuildEngine
    {
        private const string InsertGuildQuery =
            "INSERT INTO Guild (WorldId, Name, MasterCharacterId, TimeSignature) " +
            "VALUES(@worldId, @name, @masterId, @timeSignature)\r\n" +
            "SELECT CAST(@@IDENTITY AS INT)";
    }
}