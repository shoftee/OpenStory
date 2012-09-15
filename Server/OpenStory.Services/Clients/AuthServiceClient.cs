using System;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Clients
{
    /// <summary>
    /// Represents a client to the game authentication service.
    /// </summary>
    public sealed class AuthServiceClient : GameServiceClient<IAuthService>, IAuthService
    {
        /// <summary>
        /// Initialized a new instance of <see cref="AuthServiceClient"/> with the specified endpoint address.
        /// </summary>
        /// <param name="uri">The URI of the service to connect to.</param>
        public AuthServiceClient(Uri uri)
            : base(uri)
        {
        }
    }
}
