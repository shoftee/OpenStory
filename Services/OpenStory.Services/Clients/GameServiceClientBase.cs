using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Clients
{
    /// <summary>
    /// Represents a base class for game service clients.
    /// </summary>
    public abstract class GameServiceClientBase<TServiceInterface> : ClientBase<TServiceInterface>
        where TServiceInterface : class
    {
        /// <summary>
        /// Initialized a new instance of <see cref="ChannelServiceClient"/> with the specified endpoint address.
        /// </summary>
        /// <param name="endpoint">The endpoing information for the service.</param>
        protected GameServiceClientBase(ServiceEndpoint endpoint)
            : base(endpoint)
        {
        }
    }
}