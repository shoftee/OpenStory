namespace OpenStory.Server
{
    /// <summary>
    /// Provides properties for accessing Player information.
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// Gets the internal ID for the character of this player.
        /// </summary>
        int CharacterId { get; }

        /// <summary>
        /// Gets the in-game name for the character of this player.
        /// </summary>
        string CharacterName { get; }

        /// <summary>
        /// Gets the ID of the channel the player is currently in.
        /// </summary>
        int ChannelId { get; }

        /// <summary>
        /// Gets the level of the character.
        /// </summary>
        int Level { get; }

        /// <summary>
        /// Gets the job ID of the character.
        /// </summary>
        int JobId { get; }

        /// <summary>
        /// Gets the ID of the map the player is currently in.
        /// </summary>
        int MapId { get; }
    }
}