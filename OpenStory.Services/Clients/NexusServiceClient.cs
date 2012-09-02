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
        public string TryRegisterAuthService(Uri uri)
        {
            return base.Channel.TryRegisterAuthService(uri);
        }

        /// <inheritdoc />
        public bool TryUnregisterAuthService(string accessToken)
        {
            return base.Channel.TryUnregisterAuthService(accessToken);
        }

        /// <inheritdoc />
        public string TryRegisterAccountService(Uri uri)
        {
            return base.Channel.TryRegisterAccountService(uri);
        }

        /// <inheritdoc />
        public bool TryUnregisterAccountService(string accessToken)
        {
            return base.Channel.TryUnregisterAccountService(accessToken);
        }

        /// <inheritdoc />
        public string TryRegisterWorldService(Uri uri, int worldId)
        {
            return base.Channel.TryRegisterWorldService(uri, worldId);
        }

        /// <inheritdoc />
        public bool TryUnregisterWorldService(string accessToken, int worldId)
        {
            return base.Channel.TryUnregisterWorldService(accessToken, worldId);
        }

        /// <inheritdoc />
        public string TryRegisterChannelService(Uri uri, int worldId, int channelId)
        {
            return base.Channel.TryRegisterChannelService(uri, worldId, channelId);
        }

        /// <inheritdoc />
        public bool TryUnregisterChannelService(string accessToken, int worldId, int channelId)
        {
            return base.Channel.TryUnregisterChannelService(accessToken, worldId, channelId);
        }

        /// <inheritdoc />
        public IAuthService GetAuthService()
        {
            return base.Channel.GetAuthService();
        }

        /// <inheritdoc />
        public IAccountService GetAccountService()
        {
            return base.Channel.GetAccountService();
        }

        /// <inheritdoc />
        public IWorldService GetWorldService(int worldId)
        {
            return base.Channel.GetWorldService(worldId);
        }

        /// <inheritdoc />
        public IChannelService GetChannelService(int worldId, int channelId)
        {
            return base.Channel.GetChannelService(worldId, channelId);
        }

        #endregion
    }
}
