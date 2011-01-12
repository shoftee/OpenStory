using OpenStory.Common;

namespace OpenStory.Server.Registry
{
    public interface IGuild
    {
        int Id { get; }
        string Name { get; }
        int MasterCharacterId { get; }
        GuildEmblem Emblem { get; }
        bool IsFull { get; }

        string Notice { get; set; }
        int GuildPoints { get; set; }
        int Capacity { get; set; }

        string GetRankTitle(GuildRank rank);
        void SetRankTitle(GuildRank rank, string newTitle);

        bool AddGuildMember(IPlayer player);
    }
}