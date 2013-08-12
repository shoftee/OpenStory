using System.ServiceModel;
using System.ServiceModel.Discovery;
using OpenStory.Framework.Contracts;
using OpenStory.Services;
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
        private readonly IServiceConfigurationProvider serviceConfigurationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameServiceFactory"/> class.
        /// </summary>
        protected GameServiceFactory(
            INexusConnectionProvider nexusConnectionProvider, 
            IServiceConfigurationProvider serviceConfigurationProvider)
        {
            this.nexusConnectionProvider = nexusConnectionProvider;
            this.serviceConfigurationProvider = serviceConfigurationProvider;
        }

        /// <inheritdoc/>
        public ServiceHost CreateServiceHost()
        {
            var service = this.CreateService();
            var configuration = this.GetConfiguration();
            service.Configure(configuration);

            var host = new ServiceHost(service);
            host.Description.Behaviors.Add(new ServiceDiscoveryBehavior());

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
        private ServiceConfiguration GetConfiguration()
        {
            var nexusConnectionInfo = this.nexusConnectionProvider.GetConnectionInfo();
            return serviceConfigurationProvider.GetConfiguration(nexusConnectionInfo);
        }
    }
}
