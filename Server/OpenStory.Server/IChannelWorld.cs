using OpenStory.Framework.Model.Common;

namespace OpenStory.Server
{
    /// <summary>
    /// Provides properties and methods which a World Server exposes to a Channel Server.
    /// </summary>
    public interface IChannelWorld
    {
        /// <summary>
        /// Gets the ID of the World.
        /// </summary>
        int WorldId { get; }

        /// <summary>
        /// Broadcasts a message from the specified channel ID, to the specified targets.
        /// </summary>
        /// <param name="channelId">The ID of the source channel.</param>
        /// <param name="targets">The IDs of the recipients of the message.</param>
        /// <param name="data">The message to broadcast.</param>
        void BroadcastFromChannel(int channelId, CharacterKey[] targets, byte[] data);
    }
}
