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
        private readonly ILogger _logger;
        private readonly Dictionary<Guid, OsServiceConfiguration> _configurations;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryService"/> class.
        /// </summary>
        public RegistryService(ILogger logger, IDictionary<Guid, OsServiceConfiguration> configurations = null)
        {
            _logger = logger;
            _configurations = configurations != null
                ? new Dictionary<Guid, OsServiceConfiguration>(configurations)
                : new Dictionary<Guid, OsServiceConfiguration>();
        }

        #region Implementation of IRegistryService

        /// <inheritdoc />
        public Guid RegisterService(OsServiceConfiguration configuration)
        {
            var token = Guid.NewGuid();

            _configurations.Add(token, configuration);
            _logger.Info("Service registered. Token authorized: {0:N}", token);

            return token;
        }

        /// <inheritdoc />
        public void UnregisterService(Guid token)
        {
            _configurations.Remove(token);
            _logger.Info("Service unregistered. Token no longer authorized: {0:N}", token);
        }

        /// <inheritdoc />
        public Guid[] GetRegistrations()
        {
            var tokens = _configurations.Keys.ToArray();
            return tokens;
        }

        #endregion
    }
}