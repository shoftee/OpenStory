using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Registry
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    internal sealed class RegistryService : IRegistryService
    {
        private readonly Dictionary<Guid, ServiceConfiguration> configurations;

        public RegistryService()
        {
            this.configurations = new Dictionary<Guid, ServiceConfiguration>();
        }

        #region Implementation of IRegistryService

        public ServiceOperationResult TryRegisterService(ServiceConfiguration configuration, out Guid token)
        {
            Guid guid = Guid.NewGuid();
            this.configurations.Add(guid, configuration);
            token = guid;

            return new ServiceOperationResult(ServiceState.Running);
        }

        public ServiceOperationResult TryUnregisterService(Guid token)
        {
            this.configurations.Remove(token);

            return new ServiceOperationResult(ServiceState.Running);
        }

        public ServiceOperationResult TryGetRegistrations(out Guid[] tokens)
        {
            tokens = configurations.Keys.ToArray();

            return new ServiceOperationResult(ServiceState.Running);
        }

        #endregion

        #region Implementation of INexusService

        public ServiceOperationResult TryGetServiceConfiguration(Guid token, out ServiceConfiguration configuration)
        {
            configuration = this.configurations[token];

            return new ServiceOperationResult(ServiceState.Running);
        }

        #endregion
    }
}