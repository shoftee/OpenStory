using System;
using System.ServiceModel;
using OpenStory.Framework.Contracts;
using OpenStory.Services;

namespace OpenStory.Server
{
    /// <summary>
    /// Represents a class which creates game server instances.
    /// </summary>
    public abstract class GameServiceFactory : DiscoverableServiceFactory<GameServiceBase>
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

        /// <summary>
        /// Retrieves the <see cref="ServiceConfiguration"/>.
        /// </summary>
        private ServiceConfiguration GetConfiguration()
        {
            var nexusConnectionInfo = this.nexusConnectionProvider.GetConnectionInfo();
            return serviceConfigurationProvider.GetConfiguration(nexusConnectionInfo);
        }

        /// <inheritdoc/>
        public override ServiceHost CreateServiceHost()
        {
            var host = base.CreateServiceHost();

            var configuration = this.GetConfiguration();
            this.Service.Configure(configuration);

            return host;
        }

        /// <inheritdoc/>
        protected override void ConfigureServiceHost(ServiceHost serviceHost)
        {
            base.ConfigureServiceHost(serviceHost);

            serviceHost.Opened += this.OnOpened;
            serviceHost.Closing += this.OnClosing;
        }

        private void OnOpened(object sender, EventArgs e)
        {
            this.Service.Start();
        }

        private void OnClosing(object sender, EventArgs e)
        {
            this.Service.Stop();
        }
    }
}
