using OpenStory.Framework.Model.Common;

namespace OpenStory.Server
{
    /// <summary>
    /// Provides methods for a World Server to operate with a Channel Server.
    /// </summary>
    public interface IWorldChannel
    {
        /// <summary>
        /// Broadcasts a message to the specified targets.
        /// </summary>
        /// <param name="targets">The targets to send the message to.</param>
        /// <param name="data">The message to send.</param>
        void BroadcastIntoChannel(CharacterKey[] targets, byte[] data);
    }
}
