using System;
using OpenStory.Common.Tools;
using OpenStory.Framework.Contracts;

namespace OpenStory.Server.Processing
{
    /// <summary>
    /// Creates <see cref="NexusConnectionInfo"/> objects from data passed during command-line initialization.
    /// </summary>
    public sealed class EnvironmentNexusConnectionProvider : INexusConnectionProvider
    {
        private const string AccessTokenKey = @"AccessToken";

        /// <summary>
        /// Creates a <see cref="NexusConnectionInfo"/> object from the command-line parameter list.
        /// </summary>
        public NexusConnectionInfo GetConnectionInfo()
        {
            var parameters = ParameterList.FromEnvironment();
            var connection = FromParameterList(parameters);
            return connection;
        }

        private static NexusConnectionInfo FromParameterList(ParameterList parameters)
        {
            var accessTokenString = parameters[AccessTokenKey];

            Guid token;
            if (accessTokenString == null || !Guid.TryParse(accessTokenString, out token))
            {
                var error = String.Format(Errors.BootstrapTokenParseError, AccessTokenKey);
                throw new FormatException(error);
            }

            var result = new NexusConnectionInfo(token);
            return result;
        }
    }
}
