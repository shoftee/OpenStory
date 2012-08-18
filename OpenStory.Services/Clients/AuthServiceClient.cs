using OpenStory.Services.Contracts;

namespace OpenStory.Services.Clients
{
    /// <summary>
    /// Represents an Authentication Service client.
    /// </summary>
    public sealed class AuthServiceClient : GameServiceClient<IAuthService>, IAuthService
    {
        /// <summary>
        /// Gets a proxy to the Service instance.
        /// </summary>
        public AuthServiceClient()
            : base(ServiceConstants.Uris.AuthService)
        {  
        }
    }
}
