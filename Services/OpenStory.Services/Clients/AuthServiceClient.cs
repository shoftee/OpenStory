using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Clients
{
    /// <summary>
    /// Represents a client to the game authentication service.
    /// </summary>
    public sealed class AuthServiceClient : GameServiceClientBase<IAuthService>, IAuthService
    {
        /// <summary>
        /// Initialized a new instance of <see cref="AuthServiceClient"/> with the specified endpoint.
        /// </summary>
        /// <inheritdoc />
        public AuthServiceClient(ServiceEndpoint endpoint)
            : base(endpoint)
        {
        }
    }
}
