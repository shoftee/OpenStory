using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Registry
{
    /// <inheritdoc />
    [ServiceBehavior(
        ConcurrencyMode = ConcurrencyMode.Single, 
        InstanceContextMode = InstanceContextMode.Single, 
        Namespace = null)]
    public sealed class RegistryService : IRegistryService, IErrorHandler
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

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            var faultException = new FaultException(error.Message);
            var messageFault = faultException.CreateMessageFault();
            fault = Message.CreateMessage(version, messageFault, faultException.Action);
        }

        public bool HandleError(Exception error)
        {
            return false;
        }
    }
}