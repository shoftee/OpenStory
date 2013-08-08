using System.ServiceModel;
using System.ServiceModel.Description;
using OpenStory.Framework.Contracts;
using OpenStory.Services;
using OpenStory.Services.Clients;
using OpenStory.Services.Contracts;
using ServerErrors = OpenStory.Server.Errors;

namespace OpenStory.Server
{
    /// <summary>
    /// Represents a class which creates game server instances.
    /// </summary>
    public abstract class GameServiceFactory : IGenericServiceFactory
    {
        private readonly INexusConnectionProvider nexusConnectionProvider;
        private readonly IEndpointProvider endpointProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameServiceFactory"/> class.
        /// </summary>
        protected GameServiceFactory(IEndpointProvider endpointProvider, INexusConnectionProvider nexusConnectionProvider)
        {
            this.endpointProvider = endpointProvider;
            this.nexusConnectionProvider = nexusConnectionProvider;
        }

        /// <inheritdoc/>
        public ServiceHost CreateServiceHost()
        {
            var service = this.CreateService();
            var configuration = this.GetConfiguration();
            service.Configure(configuration);
            var serviceEndpoint = endpointProvider.GetEndpoint(service.GetType(), service.ServiceUri);

            var host = new ServiceHost(service);
            host.AddServiceEndpoint(serviceEndpoint);

            host.Opened += (o, e) => service.Start();
            host.Closing += (o, e) => service.Stop();

            return host;
        }

        /// <summary>
        /// Creates a <see cref="GameServiceBase"/> instance.
        /// </summary>
        protected abstract GameServiceBase CreateService();

        /// <summary>
        /// Retrieves the <see cref="ServiceConfiguration"/>.
        /// </summary>
        protected ServiceConfiguration GetConfiguration()
        {
            var nexusConnectionInfo = this.nexusConnectionProvider.GetConnectionInfo();
            var result = GetServiceConfiguration(nexusConnectionInfo);
            var configuration = CheckOperation(result).GetResult();
            return configuration;
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
