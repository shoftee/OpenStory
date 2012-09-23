using System.ServiceModel;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides methods for operating with a game world service.
    /// </summary>
    [ServiceContract(Namespace = null, Name = "WorldService", CallbackContract = typeof(IServiceStateChanged))]
    public interface IWorldService : IGameService
    {
    }
}
