using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Clients
{
    /// <summary>
    /// Represents a client to a game world service.
    /// </summary>
    public sealed class WorldServiceClient : GameServiceClientBase<IWorldService>, IWorldService
    {
        /// <summary>
        /// Initialized a new instance of <see cref="WorldServiceClient"/> with the specified endpoint.
        /// </summary>
        /// <inheritdoc />
        public WorldServiceClient(ServiceEndpoint endpoint)
            : base(endpoint)
        {
        }
    }
}
