using System.Net;
using OpenStory.Framework.Contracts;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Auth
{
    internal class AuthServerConfigurator : IServerConfigurator
    {
        public bool CheckConfiguration(ServiceConfiguration configuration, out string error)
        {
            var endpoint = configuration.Get<IPEndPoint>("Endpoint");
            if (endpoint == null)
            {
                error = "Entry point address definition missing from configuration.";
                return false;
            }

            var version = configuration.GetValue<ushort>("Version");
            if (!version.HasValue)
            {
                error = "Game version definition missing from configuration.";
                return false;
            }

            error = null;
            return true;
        }
    }
}