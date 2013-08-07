using System.Net;
using OpenStory.Framework.Contracts;
using OpenStory.Services;

namespace OpenStory.Server.Auth
{
    internal class AuthServerConfigurator : IServerConfigurator
    {
        private const string EntryPointMissing = @"Entry point address definition missing from configuration.";
        private const string VersionMissing = @"Game version definition missing from configuration.";

        public void ValidateConfiguration(ServiceConfiguration configuration)
        {
            var endpoint = configuration.Get<IPEndPoint>("Endpoint");
            if (endpoint == null)
            {
                throw new ServiceConfigurationException(EntryPointMissing);
            }

            var version = configuration.GetValue<ushort>("Version");
            if (!version.HasValue)
            {
                throw new ServiceConfigurationException(VersionMissing);
            }
        }
    }
}