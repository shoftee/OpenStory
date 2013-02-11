using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Registry
{
    /// <inheritdoc />
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    public sealed class RegistryService : IRegistryService
    {
        private readonly Dictionary<Guid, ServiceConfiguration> configurations;

        /// <summary>
        /// Initializes a new instance of <see cref="RegistryService"/>.
        /// </summary>
        public RegistryService()
        {
            this.configurations = new Dictionary<Guid, ServiceConfiguration>();
        }

        #region Implementation of IRegistryService

        /// <inheritdoc />
        public ServiceOperationResult<Guid> RegisterService(ServiceConfiguration configuration)
        {
            Guid guid = Guid.NewGuid();
            this.configurations.Add(guid, configuration);

            return new ServiceOperationResult<Guid>(guid, ServiceState.Running);
        }

        /// <inheritdoc />
        public ServiceOperationResult UnregisterService(Guid token)
        {
            this.configurations.Remove(token);

            return new ServiceOperationResult(ServiceState.Running);
        }

        /// <inheritdoc />
        public ServiceOperationResult<Guid[]> GetRegistrations()
        {
            var tokens = configurations.Keys.ToArray();

            return new ServiceOperationResult<Guid[]>(tokens, ServiceState.Running);
        }

        #endregion

        #region Implementation of INexusService

        /// <inheritdoc />
        public ServiceOperationResult<ServiceConfiguration> GetServiceConfiguration(Guid token)
        {
            ServiceConfiguration configuration;
            if (this.configurations.TryGetValue(token, out configuration))
            {
                return new ServiceOperationResult<ServiceConfiguration>(configuration, ServiceState.Running);
            }
            else
            {
                return new ServiceOperationResult<ServiceConfiguration>(OperationState.Refused, ServiceState.Running);
            }
        }

        #endregion
    }
}