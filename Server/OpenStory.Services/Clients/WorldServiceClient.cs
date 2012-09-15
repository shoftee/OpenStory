using System;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Clients
{
    /// <summary>
    /// Represents a client to a game world service.
    /// </summary>
    public sealed class WorldServiceClient : GameServiceClient<IWorldService>
    {
        /// <summary>
        /// Initialized a new instance of <see cref="WorldServiceClient"/> with the specified endpoint address.
        /// </summary>
        /// <param name="uri">The URI of the service to connect to.</param>
        public WorldServiceClient(Uri uri)
            : base(uri)
        {
        }
    }
}
