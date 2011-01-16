using System;

namespace OpenStory.Server.Registry
{
    /// <summary>
    /// Represents a player that is in a Game Messenger session.
    /// </summary>
    public class MessengerMember : IEquatable<MessengerMember>
    {
        /// <summary>
        /// Initializes a new instance of MessengerMember 
        /// using information from a <see cref="IPlayer"/> object,
        /// and optionally assigning it a position in the Messenger window.
        /// </summary>
        /// <param name="player">The <see cref="IPlayer"/> object to base this instance on.</param>
        /// <param name="position">The position in the Messenger window for this MessengerMember. The default value is 0.</param>
        public MessengerMember(IPlayer player, int position = 0)
        {
            this.Position = position;
            this.CharacterId = player.CharacterId;
            this.CharacterName = player.CharacterName;
            this.ChannelId = player.ChannelId;
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
        /// Gets the player's position in the Messenger window.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Gets the ID of the channel the player is currently in.
        /// </summary>
        public int ChannelId { get; set; }

        #region IEquatable<MessengerMember> Members

        /// <summary>
        /// Indicates whether the current MessengerMember is the same player as another PartyMember.
        /// </summary>
        /// <remarks>
        /// This method compares the MessengerMember instances by their <see cref="CharacterId"/> properties.
        /// </remarks>
        /// <returns>
        /// true if the current MessengerMember is based on the same player as <paramref name="other"/>; otherwise, false.
        /// If <paramref name="other"/> is null, this method returns false.
        /// </returns>
        /// <param name="other">A MessengerMember instance to compare with.</param>
        public bool Equals(MessengerMember other)
        {
            if (other == null) return false;
            return this.CharacterId == other.CharacterId;
        }

        #endregion
    }
}