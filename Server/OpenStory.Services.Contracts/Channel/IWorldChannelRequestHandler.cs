using System.ServiceModel;
using OpenStory.Framework.Model.Common;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides methods for a World Server to operate with a Channel Server.
    /// </summary>
    [ServiceContract(Namespace = null, Name = "WorldToChannelService")]
    public interface IWorldChannelRequestHandler
    {
        /// <summary>
        /// Broadcasts a message to the specified targets.
        /// </summary>
        /// <param name="targets">The targets to send the message to.</param>
        /// <param name="data">The message to send.</param>
        [OperationContract]
        void BroadcastIntoChannel(CharacterKey[] targets, byte[] data);
    }
}
