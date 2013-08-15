using System.ServiceModel;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Registry
{
    internal sealed class RegistryServiceFactory : DiscoverableServiceFactory<IRegistryService>
    {
        protected override void ConfigureServiceHost(ServiceHost serviceHost)
        {
            base.ConfigureServiceHost(serviceHost);
            serviceHost.AddServiceEndpoint(
                typeof(IRegistryService),
                new NetTcpBinding(SecurityMode.Transport),
                "net.tcp://localhost/OpenStory/Registry");
        }

        protected override IRegistryService CreateService()
        {
            var registryService = new RegistryService();
            return registryService;
        }
    }
}