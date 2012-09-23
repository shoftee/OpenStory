using System;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Management
{
    class RegistryService : IRegistryService
    {
        #region Implementation of IRegistryService

        public ServiceState TryRegisterAuthService(Uri uri, out Guid token)
        {
            throw new NotImplementedException();
        }

        public ServiceState TryRegisterAccountService(Uri uri, out Guid token)
        {
            throw new NotImplementedException();
        }

        public ServiceState TryRegisterWorldService(Uri uri, int worldId, out Guid token)
        {
            throw new NotImplementedException();
        }

        public ServiceState TryRegisterChannelService(Uri uri, int worldId, int channelId, out Guid token)
        {
            throw new NotImplementedException();
        }

        public ServiceState TryUnregisterService(Guid registrationToken)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}