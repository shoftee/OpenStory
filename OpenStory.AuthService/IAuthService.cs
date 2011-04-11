using System;
using System.ServiceModel;
using OpenStory.Server.Common;

namespace OpenStory.AuthService
{
    /// <summary>
    /// Provides methods for accessing and managing the Authentication Service.
    /// </summary>
    [ServiceContract(Namespace = null)]
    public interface IAuthService : IGameService
    {
    }
}