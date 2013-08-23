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
        public Guid RegisterService(ServiceConfiguration configuration)
        {
            Guid guid = Guid.NewGuid();
            this.configurations.Add(guid, configuration);

            return guid;
        }

        /// <inheritdoc />
        public void UnregisterService(Guid token)
        {
            this.configurations.Remove(token);
        }

        /// <inheritdoc />
        public Guid[] GetRegistrations()
        {
            var tokens = this.configurations.Keys.ToArray();
            return tokens;
        }

        #endregion

        #region Implementation of INexusService

        /// <inheritdoc />
        public ServiceConfiguration GetServiceConfiguration(Guid token)
        {
            ServiceConfiguration configuration;
            if (!this.configurations.TryGetValue(token, out configuration))
            {
                var exception = new InvalidOperationException("This service access token is not authorized.");
                throw new FaultException<InvalidOperationException>(exception);
            }

            return configuration;
        }

        #endregion
    }
}