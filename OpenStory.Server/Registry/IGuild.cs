using OpenStory.Common;

namespace OpenStory.Server.Registry
{
    /// <summary>
    /// Provides properties and methods for a game Guild.
    /// </summary>
    public interface IGuild
    {
        /// <summary>
        /// Gets the internal ID for the Guild.
        /// </summary>
        int Id { get; }
        
        /// <summary>
        /// Gets the guild name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the ID of the Character that is the master of the Guild.
        /// </summary>
        int MasterCharacterId { get; }

        /// <summary>
        /// Gets the GuildEmblem object for the Guild.
        /// </summary>
        GuildEmblem Emblem { get; }

        /// <summary>
        /// Denotes whether the guild is full or not.
        /// </summary>
        bool IsFull { get; }

        /// <summary>
        /// Gets or sets the guild notice.
        /// </summary>
        string Notice { get; set; }

        /// <summary>
        /// Gets or sets the number of guild points.
        /// </summary>
        int GuildPoints { get; set; }

        /// <summary>
        /// Gets or sets the guild's member capacity.
        /// </summary>
        int Capacity { get; set; }

        /// <summary>
        /// Gets the title for a given guild rank.
        /// </summary>
        /// <param name="rank">The rank to get the title of.</param>
        /// <returns>The name of the title for the given rank.</returns>
        string GetRankTitle(GuildRank rank);

        /// <summary>
        /// Sets the title for a given guild rank.
        /// </summary>
        /// <param name="rank">The rank to set the title of.</param>
        /// <param name="newTitle">The new title for the given rank.</param>
        void SetRankTitle(GuildRank rank, string newTitle);

        /// <summary>
        /// Adds a new member to the guild.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        bool AddGuildMember(IPlayer player);
    }
}