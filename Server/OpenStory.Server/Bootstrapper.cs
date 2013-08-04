using System;
using OpenStory.Framework.Contracts;
using OpenStory.Server.Processing;
using OpenStory.Services;
using OpenStory.Services.Clients;
using OpenStory.Services.Contracts;

namespace OpenStory.Server
{
    /// <summary>
    /// Bootstrapper.
    /// </summary>
    public sealed class Bootstrapper
    {
        private readonly IGameServiceFactory gameServiceFactory;
        private readonly INexusConnectionProvider nexusConnectionProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        public Bootstrapper(
            IGameServiceFactory gameServiceFactory, 
            INexusConnectionProvider nexusConnectionProvider)
        {
            this.gameServiceFactory = gameServiceFactory;
            this.nexusConnectionProvider = nexusConnectionProvider;
        }

        /// <summary>
        /// Starts the service.
        /// </summary>
        /// <exception cref="BootstrapException">Thrown if there are any errors during the initialization process.</exception>
        /// <returns>an instance of <see cref="GameServiceBase"/>, or <see langword="null"/> if there was an error.</returns>
        public GameServiceBase Service()
        {
            try
            {
                var nexusConnection = this.nexusConnectionProvider.GetConnection();
                var result = GetServiceConfiguration(nexusConnection);
                var service = this.gameServiceFactory.CreateService();

                service.Configure(result.GetResult());
                
                return service;
            }
            catch (BootstrapException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new BootstrapException("There was a problem during the bootstrapping process.", exception);
            }
        }

        private static ServiceOperationResult<ServiceConfiguration> GetServiceConfiguration(NexusConnection info)
        {
            using (var nexus = new NexusServiceClient(info.NexusUri))
            {
                var result = nexus.GetServiceConfiguration(info.AccessToken);
                return result;
            }
        }

        private static void CheckOperationResult(IServiceOperationResult result)
        {
            switch (result.OperationState)
            {
                case OperationState.FailedLocally:
                    var couldNotConnectMessage = string.Format(Errors.BootstrapCouldNotConnectToNexus, result.Error);
                    throw new BootstrapException(couldNotConnectMessage);

                case OperationState.Refused:
                    var requestRefusedMessage = Errors.BootstrapRequestRefused;
                    throw new BootstrapException(requestRefusedMessage);

                default:
                    var genericErrorMessage = string.Format(Errors.BootstrapNexusGenericError, result.Error);
                    throw new BootstrapException(genericErrorMessage);
            }
        }
    }
}
