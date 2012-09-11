using System.Collections.Generic;

namespace OpenStory.Server.Channel
{
    /// <summary>
    /// Provides methods for operating with a Channel Server.
    /// </summary>
    internal interface IChannelServer : IGameServer
    {
        /// <summary>
        /// Gets the World Server link object for this Channel Server.
        /// </summary>
        IChannelWorld World { get; }

        /// <summary>
        /// Broadcasts a message to the whole world server.
        /// </summary>
        /// <param name="sourceId">The ID of the sender.</param>
        /// <param name="targetIds">The IDs of the recepients of the message.</param>
        /// <param name="data">The message to broadcast.</param>
        void BroadcastToWorld(int sourceId, IEnumerable<int> targetIds, byte[] data);
    }
}
