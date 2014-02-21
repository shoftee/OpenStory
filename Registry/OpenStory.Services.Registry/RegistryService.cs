using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Ninject.Extensions.Logging;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Registry
{
    /// <inheritdoc />
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    sealed class RegistryService : IRegistryService
    {
        private readonly ILogger logger;
        private readonly Dictionary<Guid, OsServiceConfiguration> configurations;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryService"/> class.
        /// </summary>
        public RegistryService(ILogger logger, IDictionary<Guid, OsServiceConfiguration> configurations = null)
        {
            this.logger = logger;
            this.configurations = configurations != null
                ? new Dictionary<Guid, OsServiceConfiguration>(configurations)
                : new Dictionary<Guid, OsServiceConfiguration>();
        }

        #region Implementation of IRegistryService

        /// <inheritdoc />
        public Guid RegisterService(OsServiceConfiguration configuration)
        {
            var token = Guid.NewGuid();

            this.configurations.Add(token, configuration);
            this.logger.Info("Service registered. Token authorized: {0:N}", token);

            return token;
        }

        /// <inheritdoc />
        public void UnregisterService(Guid token)
        {
            this.configurations.Remove(token);
            this.logger.Info("Service unregistered. Token no longer authorized: {0:N}", token);
        }

        /// <inheritdoc />
        public Guid[] GetRegistrations()
        {
            var tokens = this.configurations.Keys.ToArray();
            return tokens;
        }

        #endregion
    }
}