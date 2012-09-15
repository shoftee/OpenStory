using System.ServiceModel;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides methods for operating with a game channel service.
    /// </summary>
    [ServiceContract(Namespace = null, Name = "ChannelService", CallbackContract = typeof(IServiceStateChanged))]
    public interface IChannelService : IGameService
    {
    }
}
