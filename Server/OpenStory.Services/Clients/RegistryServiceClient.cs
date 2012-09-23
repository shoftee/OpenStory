using System;
using System.ServiceModel;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Clients
{
    /// <summary>
    /// Represents a client to a game nexus service.
    /// </summary>
    public sealed class RegistryServiceClient : ClientBase<IRegistryService>, IRegistryService
    {
        /// <summary>
        /// Initialized a new instance of <see cref="RegistryServiceClient"/> with the specified endpoint address.
        /// </summary>
        /// <param name="uri">The URI of the service to connect to.</param>
        public RegistryServiceClient(Uri uri)
            : base(new NetTcpBinding(SecurityMode.Transport), new EndpointAddress(uri))
        {
        }

        #region IRegistryService Members

        /// <inheritdoc />
        public ServiceState TryRegisterAuthService(Uri uri, out Guid token)
        {
            return base.Channel.TryRegisterAuthService(uri, out token);
        }

        /// <inheritdoc />
        public ServiceState TryRegisterAccountService(Uri uri, out Guid token)
        {
            return base.Channel.TryRegisterAccountService(uri, out token);
        }

        /// <inheritdoc />
        public ServiceState TryRegisterWorldService(Uri uri, int worldId, out Guid token)
        {
            return base.Channel.TryRegisterWorldService(uri, worldId, out token);
        }

        /// <inheritdoc />
        public ServiceState TryRegisterChannelService(Uri uri, int worldId, int channelId, out Guid token)
        {
            return base.Channel.TryRegisterChannelService(uri, worldId, channelId, out token);
        }

        /// <inheritdoc />
        public ServiceState TryUnregisterService(Guid registrationToken)
        {
            return base.Channel.TryUnregisterService(registrationToken);
        }

        #endregion
    }
}
