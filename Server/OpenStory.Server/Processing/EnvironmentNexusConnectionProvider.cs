using System;
using Ninject.Activation;
using OpenStory.Common;
using OpenStory.Framework.Contracts;

namespace OpenStory.Server.Processing
{
    /// <summary>
    /// Creates <see cref="NexusConnectionInfo"/> objects from data passed during command-line initialization.
    /// </summary>
    public sealed class EnvironmentNexusConnectionProvider : Provider<NexusConnectionInfo>
    {
        private const string AccessTokenKey = @"AccessToken";

        /// <inheritdoc/>
        protected override NexusConnectionInfo CreateInstance(IContext context)
        {
            var parameters = ParameterList.FromEnvironment();
            var accessTokenString = parameters[AccessTokenKey];

            Guid token;
            if (!Guid.TryParse(accessTokenString, out token))
            {
                var error = String.Format(ServerStrings.BootstrapTokenParseError, AccessTokenKey);
                throw new FormatException(error);
            }

            var info = new NexusConnectionInfo(token);
            return info;
        }
    }
}
