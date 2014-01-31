using System.Collections.Generic;
using System.ServiceModel;
using OpenStory.Framework.Model.Common;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides methods for a World Server to operate with a Channel Server.
    /// </summary>
    [ServiceContract(Namespace = null, Name = "WorldToChannelService")]
    public interface IWorldToChannelRequestHandler
    {
        /// <summary>
        /// Gets the numeric channel identifier.
        /// </summary>
        int ChannelId { [OperationContract] get; }

        /// <summary>
        /// Gets a non-negative integer denoting how populated the channel is.
        /// </summary>
        int Population { [OperationContract] get; }

        /// <summary>
        /// Broadcasts a message to the specified targets.
        /// </summary>
        /// <param name="targets">The targets to send the message to.</param>
        /// <param name="data">The message to send.</param>
        [OperationContract]
        void BroadcastIntoChannel(IEnumerable<CharacterKey> targets, byte[] data);
    }
}
