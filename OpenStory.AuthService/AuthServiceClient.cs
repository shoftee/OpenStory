using System.ServiceModel;
using System.ServiceModel.Channels;
using OpenStory.Server;

namespace OpenStory.AuthService
{
    /// <summary>
    /// Represents an Authentication Service client.
    /// </summary>
    public class AuthServiceClient : ClientBase<IAuthService>, IAuthService
    {
        /// <summary>
        /// Gets a proxy to the Service instance.
        /// </summary>
        public AuthServiceClient()
            : base(ServiceHelpers.GetBinding(), new EndpointAddress(ServerConstants.AuthServiceUri))
        {  
        }

        public void Start()
        {
            this.Channel.Start();
        }

        public void Stop()
        {
            this.Channel.Stop();
        }
    }
}
