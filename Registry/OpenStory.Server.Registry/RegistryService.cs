using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using OpenStory.Services;
using OpenStory.Services.Contracts;
using OpenStory.Services.Registry;

namespace OpenStory.Server.Registry
{
    /// <inheritdoc />
    [ServiceBehavior(
        ConcurrencyMode = ConcurrencyMode.Single,
        InstanceContextMode = InstanceContextMode.Single,
        Namespace = null)]
    internal sealed class RegistryService : IRegistryService
    {
        private readonly Dictionary<Guid, ServiceConfiguration> configurations;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryService"/> class.
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
            var tokens = this.configurations.Keys.ToArray();

            return new ServiceOperationResult<Guid[]>(tokens, ServiceState.Running);
        }

        #endregion

        #region Implementation of INexusService

        /// <inheritdoc />
        public ServiceOperationResult<ServiceConfiguration> GetServiceConfiguration(Guid token)
        {
            ServiceConfiguration configuration;
            if (!this.configurations.TryGetValue(token, out configuration))
            {
                throw new InvalidOperationException("This service access token is not authorized.");
            }

            return new ServiceOperationResult<ServiceConfiguration>(configuration, ServiceState.Running);
        }

        #endregion
    }
}