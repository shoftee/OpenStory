using OpenStory.Framework.Model.Common;
using OpenStory.Server.Processing;

namespace OpenStory.Server
{
    /// <summary>
    /// Provides properties of a Player.
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// Gets the unique identifier for the character of this player.
        /// </summary>
        CharacterKey Key { get; }

        /// <summary>
        /// Gets the appearance information for the character of this player.
        /// </summary>
        CharacterAppearance Appearance { get; }

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

        /// <summary>
        /// Gets the client instance of the player.
        /// </summary>
        ClientBase Client { get; }
    }
}
