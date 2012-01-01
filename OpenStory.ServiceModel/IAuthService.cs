using System.ServiceModel;

namespace OpenStory.ServiceModel
{
    /// <summary>
    /// Provides methods for accessing and managing the Authentication Service.
    /// </summary>
    [ServiceContract(Namespace = null)]
    public interface IAuthService : IGameService
    {
    }
}