using System;
using OpenMaple.Client;

namespace OpenMaple.Handling.World
{
    class GuildMember
    {
        public int GuildId { get; private set; }

        public int CharacterId { get; private set; }
        public string Name { get; private set; }
        public int Level { get; private set; }
        public int JobId { get; private set; }
        public GuildRank Rank { get; private set; }

        public bool IsOnline { get; private set; }
        public byte ChannelId { get; private set; }
    }
}