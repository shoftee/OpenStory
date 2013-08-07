using System.Collections.Generic;
using OpenStory.Framework.Model.Common;

namespace OpenStory.Server.Channel
{
    /// <summary>
    /// Provides methods for operating with a Channel Server.
    /// </summary>
    internal interface IChannelServer
    {
        /// <summary>
        /// Gets the World Server link object for this Channel Server.
        /// </summary>
        IChannelWorld World { get; }

        /// <summary>
        /// Broadcasts a message to the whole world server.
        /// </summary>
        /// <param name="sourceKey">The ID of the sender.</param>
        /// <param name="targets">The IDs of the recipients of the message.</param>
        /// <param name="data">The message to broadcast.</param>
        void BroadcastToWorld(CharacterKey sourceKey, IEnumerable<CharacterKey> targets, byte[] data);
    }
}
