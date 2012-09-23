using System;
using System.ServiceModel;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Clients
{
    /// <summary>
    /// Represents a client to the nexus service.
    /// </summary>
    public sealed class NexusServiceClient : ClientBase<INexusService>, INexusService
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NexusServiceClient"/>.
        /// </summary>
        /// <param name="uri">The URI of the service to connect to.</param>
        public NexusServiceClient(Uri uri)
            : base(new NetTcpBinding(SecurityMode.Transport), new EndpointAddress(uri))
        {
        }

        #region Implementation of INexusService

        /// <inheritdoc />
        public ServiceOperationResult TryGetAccountServiceUri(Guid accessToken, out Uri uri)
        {
            return base.Channel.TryGetAccountServiceUri(accessToken, out uri);
        }

        /// <inheritdoc />
        public ServiceOperationResult TryGetAuthServiceUri(Guid accessToken, out Uri uri)
        {
            return base.Channel.TryGetAuthServiceUri(accessToken, out uri);
        }

        /// <inheritdoc />
        public ServiceOperationResult TryGetChannelServiceUri(Guid accessToken, out Uri uri)
        {
            return base.Channel.TryGetChannelServiceUri(accessToken, out uri);
        }

        /// <inheritdoc />
        public ServiceOperationResult TryGetWorldServiceUri(Guid accessToken, out Uri uri)
        {
            return base.Channel.TryGetWorldServiceUri(accessToken, out uri);
        }

        /// <inheritdoc />
        public ServiceOperationResult TryGetServiceConfiguration(Guid accessToken, out ServiceConfiguration configuration)
        {
            return base.Channel.TryGetServiceConfiguration(accessToken, out configuration);
        }

        #endregion
    }
}