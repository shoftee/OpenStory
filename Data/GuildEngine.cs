using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMaple.Data
{
    static class GuildEngine
    {
        const string InsertGuildQuery = 
            "INSERT INTO Guild (WorldId, Name, MasterCharacterId, TimeSignature) " +
            "VALUES(@worldId, @name, @masterId, @timeSignature)\r\n" +
            "SELECT CAST(@@IDENTITY AS INT)";
    }
}
