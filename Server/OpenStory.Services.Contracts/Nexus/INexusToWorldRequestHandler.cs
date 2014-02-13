using System.ServiceModel;
using OpenStory.Common.Game;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides properties and methods which a World Server exposes to a Nexus Server.
    /// </summary>
    [ServiceContract(Namespace = null, Name = "NexusToWorldService")]
    public interface INexusToWorldRequestHandler
    {
        /// <summary>
        /// Gets the ID of the World.
        /// </summary>
        int WorldId { [OperationContract] get; }

        /// <summary>
        /// Retrieves the world details object for this server.
        /// </summary>
        IWorld GetDetails();
    }
}
