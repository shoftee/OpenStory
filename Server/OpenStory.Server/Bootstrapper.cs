using System;
using System.Threading;
using Ninject.Extensions.Logging;
using OpenStory.Framework.Contracts;
using OpenStory.Server.Processing;
using OpenStory.Services.Clients;
using OpenStory.Services.Contracts;
using ServerErrors = OpenStory.Server.Errors;

namespace OpenStory.Server
{
    /// <summary>
    /// Bootstrapper.
    /// </summary>
    public sealed class Bootstrapper
    {
        private readonly IGameServiceFactory gameServiceFactory;
        private readonly INexusConnectionProvider nexusConnectionProvider;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        public Bootstrapper(
            IGameServiceFactory gameServiceFactory, 
            INexusConnectionProvider nexusConnectionProvider,
            ILogger logger)
        {
            this.gameServiceFactory = gameServiceFactory;
            this.nexusConnectionProvider = nexusConnectionProvider;
            this.logger = logger;
        }

        /// <summary>
        /// Starts the whole thing.
        /// </summary>
        /// <exception cref="BootstrapException">Thrown if there are any errors during the initialization process.</exception>
        public void Start()
        {
            try
            {
                var nexusConnectionInfo = this.nexusConnectionProvider.GetConnectionInfo();
                var result = GetServiceConfiguration(nexusConnectionInfo);
                var configuration = CheckOperation(result).GetResult();

                var service = this.gameServiceFactory.CreateService();
                service.Configure(configuration);

                using (service)
                {
                    service.Start();
                    Thread.Sleep(Timeout.Infinite);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, ServerErrors.BootstrapGenericError);
            }
        }

        private static ServiceOperationResult<ServiceConfiguration> GetServiceConfiguration(NexusConnectionInfo nexusConnectionInfo)
        {
            using (var client = new NexusServiceClient(nexusConnectionInfo.NexusUri))
            {
                var result = client.GetServiceConfiguration(nexusConnectionInfo.AccessToken);
                return result;
            }
        }

        private static TServiceOperationResult CheckOperation<TServiceOperationResult>(TServiceOperationResult result)
            where TServiceOperationResult : IServiceOperationResult
        {
            switch (result.OperationState)
            {
                case OperationState.FailedLocally:
                    var couldNotConnectMessage = string.Format(ServerErrors.BootstrapCouldNotConnectToNexus);
                    throw new BootstrapException(couldNotConnectMessage, result.Error);

                case OperationState.Refused:
                    var requestRefusedMessage = ServerErrors.BootstrapRequestRefused;
                    throw new BootstrapException(requestRefusedMessage);

                case OperationState.FailedRemotely:
                    var genericErrorMessage = string.Format(ServerErrors.BootstrapNexusGenericError);
                    throw new BootstrapException(genericErrorMessage, result.Error);
            }

            return result;
        }
    }
}
