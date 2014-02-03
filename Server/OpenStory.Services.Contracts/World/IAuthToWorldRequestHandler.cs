using System.ServiceModel;
using OpenStory.Common.Game;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides properties and methods which a World Server exposes to an Auth Server.
    /// </summary>
    [ServiceContract(Namespace = null, Name = "AuthToWorldService")]
    public interface IAuthToWorldRequestHandler 
    {
        /// <summary>
        /// Gets the ID of the World.
        /// </summary>
        int WorldId { [OperationContract] get; }

        /// <summary>
        /// Retrieves the information of the specified world.
        /// </summary>
        [OperationContract]
        IWorld GetWorldInfo();
    }
}
