namespace OpenStory.ServiceModel
{
    /// <summary>
    /// Represents an Authentication Service client.
    /// </summary>
    public class AuthServiceClient : GameServiceClient<IAuthService>, IAuthService
    {
        /// <summary>
        /// Gets a proxy to the Service instance.
        /// </summary>
        public AuthServiceClient()
            : base(ServerConstants.Uris.AuthService)
        {  
        }
    }
}
