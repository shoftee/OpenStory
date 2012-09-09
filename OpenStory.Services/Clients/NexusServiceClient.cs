using System;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Clients
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NexusServiceClient : GameServiceClient<INexusService>, INexusService
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NexusServiceClient"/>.
        /// </summary>
        public NexusServiceClient()
            : base(ServiceConstants.Uris.NexusService)
        {
        }

        #region INexusService Members

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
