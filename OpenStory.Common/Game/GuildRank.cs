using System;

namespace OpenStory.Common.Game
{
    /// <summary>
    /// A guild rank.
    /// </summary>
    [Serializable]
    public enum GuildRank : byte
    {
        /// <summary>
        /// Default value
        /// </summary>
        None = 0,

        /// <summary>
        /// The rank given to the leader of a guild.
        /// </summary>
        Master = 1,

        /// <summary>
        /// The rank given to the assistants of a guild's leader.
        /// </summary>
        JuniorMaster = 2,

        /// <summary>
        /// Generic high-ranked member.
        /// </summary>
        HighMember = 3,

        /// <summary>
        /// Generic middle-ranked member.
        /// </summary>
        MediumMember = 4,

        /// <summary>
        /// Generic low-ranked member.
        /// </summary>
        LowMember = 5
    }
}