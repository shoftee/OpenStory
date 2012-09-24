using System;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Management
{
    class RegistryService : IRegistryService
    {
        #region Implementation of IRegistryService

        public ServiceOperationResult TryRegisterService(ServiceConfiguration configuration, out Guid token)
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