using System;
using OpenStory.Services;
using OpenStory.Services.Registry;

namespace OpenStory.Server.Registry
{
    internal sealed class RegistryServiceHostFactory : DiscoverableServiceHostFactory<IRegistryService>
    {
        public RegistryServiceHostFactory(IServiceFactory<IRegistryService> serviceFactory)
            : base(serviceFactory)
        {
        }

        protected override ServiceConfiguration GetConfiguration()
        {
            var defaultUri = new Uri("net.tcp://localhost/OpenStory/Registry");
            var configuration = ServiceConfiguration.Registry(defaultUri);
            return configuration;
        }
    }
}