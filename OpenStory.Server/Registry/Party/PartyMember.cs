using System;

namespace OpenStory.Server.Registry.Party
{
    /// <summary>
    /// Represents a player that is a member of a party.
    /// </summary>
    public class PartyMember : IEquatable<PartyMember>
    {
        /// <summary>
        /// Initializes a new instance of PartyMember 
        /// using information from a <see cref="IPlayer"/> object.
        /// </summary>
        /// <param name="player">The player that this PartyMember instance is based on.</param>
        public PartyMember(IPlayer player)
        {
            this.IsOnline = true;

            this.CharacterId = player.CharacterId;
            this.CharacterName = player.CharacterName;
            this.Level = player.Level;
            this.ChannelId = player.ChannelId;
            this.JobId = player.JobId;
            this.MapId = player.MapId;
        }

        /// <summary>
        /// Gets the Character ID of the player.
        /// </summary>
        public int CharacterId { get; private set; }

        /// <summary>
        /// Gets the player's in-game name.
        /// </summary>
        public string CharacterName { get; private set; }

        /// <summary>
        /// Gets the level of the player.
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// Gets the ID of the channel the player is currently in.
        /// </summary>
        public int ChannelId { get; private set; }

        /// <summary>
        /// Gets the job ID of the player.
        /// </summary>
        public int JobId { get; private set; }

        /// <summary>
        /// Gets the ID of the map the player is currently in.
        /// </summary>
        public int MapId { get; private set; }

        /// <summary>
        /// Denotes whether the player is online or not.
        /// </summary>
        public bool IsOnline { get; private set; }

        #region IEquatable<PartyMember> Members

        /// <summary>
        /// Indicates whether the current PartyMember is the same player as another PartyMember.
        /// </summary>
        /// <remarks>
        /// This method compares the PartyMember instances by their <see cref="CharacterId"/> properties.
        /// </remarks>
        /// <returns>
        /// true if the current PartyMember is based on the same player as <paramref name="other"/>; otherwise, false.
        /// If <paramref name="other"/> is <c>null</c>, this method returns false.
        /// </returns>
        /// <param name="other">A PartyMember instance to compare with.</param>
        public bool Equals(PartyMember other)
        {
            if (other == null) return false;
            return this.CharacterId == other.CharacterId;
        }

        #endregion
    }
}