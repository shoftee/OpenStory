using System;
using System.ServiceModel;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Registry
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    internal sealed class RegistryService : IRegistryService
    {
        public RegistryService()
        {
        }

        #region Implementation of IRegistryService

        public ServiceOperationResult TryRegisterService(ServiceConfiguration configuration, out Guid token)
        {
            throw new NotImplementedException();
        }

        public ServiceOperationResult TryUnregisterService(Guid registrationToken)
        {
            throw new NotImplementedException();
        }

        public ServiceOperationResult TryGetRegistrations(out Guid[] tokens)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Implementation of INexusService

        public ServiceOperationResult TryGetServiceConfiguration(Guid accessToken, out ServiceConfiguration configuration)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}