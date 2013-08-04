using System;
using OpenStory.Common.Tools;
using OpenStory.Framework.Contracts;

namespace OpenStory.Server.Processing
{
    /// <summary>
    /// Creates <see cref="NexusConnection"/> objects from data passed during command-line initialization.
    /// </summary>
    public sealed class ParameterListNexusConnectionProvider : INexusConnectionProvider
    {
        private const string AccessTokenKey = @"AccessToken";
        private const string NexusUriKey = @"NexusUri";

        /// <summary>
        /// Creates a <see cref="NexusConnection"/> object from the command-line parameter list.
        /// </summary>
        public NexusConnection GetConnection()
        {
            var parameters = ParameterList.FromEnvironment();
            var connection = FromParameterList(parameters);
            return connection;
        }

        private static NexusConnection FromParameterList(ParameterList parameters)
        {
            var nexusUriString = parameters[NexusUriKey];

            Uri uri;
            if (nexusUriString == null || !Uri.TryCreate(nexusUriString, UriKind.Absolute, out uri))
            {
                var error = String.Format(Errors.BootstrapUriParseError, NexusUriKey);
                throw new FormatException(error);
            }

            var accessTokenString = parameters[AccessTokenKey];

            Guid token;
            if (accessTokenString == null || !Guid.TryParse(accessTokenString, out token))
            {
                var error = String.Format(Errors.BootstrapTokenParseError, AccessTokenKey);
                throw new FormatException(error);
            }

            var result = new NexusConnection(token, uri);
            return result;
        }
    }
}
