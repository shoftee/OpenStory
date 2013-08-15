using System.ServiceModel;
using OpenStory.Framework.Contracts;
using OpenStory.Services;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Auth
{
    class AuthServiceFactory : GameServiceFactory
    {
        public AuthServiceFactory(INexusConnectionProvider nexusConnectionProvider, IServiceConfigurationProvider serviceConfigurationProvider)
            : base(nexusConnectionProvider, serviceConfigurationProvider)
        {
        }

        protected override void ConfigureServiceHost(ServiceHost serviceHost)
        {
            base.ConfigureServiceHost(serviceHost);
            serviceHost.AddServiceEndpoint(
                typeof(IAuthService), 
                new NetTcpBinding(SecurityMode.Transport), 
                "net.tcp://localhost/OpenStory/Auth");
        }

        protected override GameServiceBase CreateService()
        {
            return new AuthService();
        }
    }
}
