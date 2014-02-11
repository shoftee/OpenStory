using System.Collections.Generic;
using System.ServiceModel;
using OpenStory.Common.Game;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides properties and methods which a World Server exposes to an Auth Server.
    /// </summary>
    [ServiceContract(Namespace = null, Name = "AuthToNexusService")]
    public interface IAuthToNexusRequestHandler
    {
        /// <summary>
        /// Retrieves the information of the specified world.
        /// </summary>
        [OperationContract]
        IEnumerable<IWorld> GetWorlds();
    }
}
