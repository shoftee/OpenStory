using System;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Management
{
    class RegistryService : IRegistryService
    {
        #region Implementation of IRegistryService

        public ServiceOperationResult TryRegisterAuthService(Uri uri, out Guid token)
        {
            throw new NotImplementedException();
        }

        public ServiceOperationResult TryRegisterAccountService(Uri uri, out Guid token)
        {
            throw new NotImplementedException();
        }

        public ServiceOperationResult TryRegisterWorldService(Uri uri, int worldId, out Guid token)
        {
            throw new NotImplementedException();
        }

        public ServiceOperationResult TryRegisterChannelService(Uri uri, int worldId, int channelId, out Guid token)
        {
            throw new NotImplementedException();
        }

        public ServiceOperationResult TryUnregisterService(Guid registrationToken)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}