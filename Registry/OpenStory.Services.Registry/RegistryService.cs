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
        
        private readonly Dictionary<Guid, Uri> uris;
        private readonly Dictionary<Guid, ServiceConfiguration> configurations;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryService"/> class.
        /// </summary>
        public RegistryService(ILogger logger)
        {
            this.logger = logger;

            this.uris = new Dictionary<Guid, Uri>();
            this.configurations = new Dictionary<Guid, ServiceConfiguration>();
        }

        #region Implementation of IRegistryService

        /// <inheritdoc />
        public Guid RegisterService(ServiceConfiguration configuration)
        {
            Guid token = Guid.NewGuid();

            this.configurations.Add(token, configuration);
            this.logger.Info("Service registered. Token authorized: {0:N}", token);

            return token;
        }

        /// <inheritdoc />
        public void UnregisterService(Guid token)
        {
            this.uris.Remove(token);
            this.logger.Info("Service unregistered. Token no longer authorized: {0:N}", token);
        }

        /// <inheritdoc />
        public Guid[] GetRegistrations()
        {
            var tokens = this.uris.Keys.ToArray();
            return tokens;
        }

        #endregion

        #region Implementation of INexusService

        /// <inheritdoc />
        public Uri GetServiceUri(Guid token)
        {
            Uri uri;
            if (!this.uris.TryGetValue(token, out uri))
            {
                this.logger.Debug("Refused unauthorized token: {0:N}", token);

                var exception = new InvalidOperationException("This service access token is not authorized.");
                throw new FaultException<InvalidOperationException>(exception);
            }

            return uri;
        }

        #endregion
    }
}