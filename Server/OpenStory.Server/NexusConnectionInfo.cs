using System;
using OpenStory.Common.Tools;

namespace OpenStory.Server
{
    /// <summary>
    /// Represents connection information for a nexus service.
    /// </summary>
    public sealed class NexusConnectionInfo
    {
        private const string AccessTokenKey = "AccessToken";
        private const string NexusUriKey = "NexusUri";

        private const string TokenParseErrorFormat =
            "Could not parse nexus access token. Make sure you've entered a valid GUID value under the parameter '{0}' (case-sensitive).";

        private const string NexusUriParseErrorFormat =
            "Could not parse nexus URI string. Make sure you've entered a valid service URI under the parameter '{0}' (case-sensitive).";

        /// <summary>
        /// Gets the access token required to communicate with the Nexus service.
        /// </summary>
        public Guid AccessToken { get; private set; }

        /// <summary>
        /// Gets the URI of the Nexus service.
        /// </summary>
        public Uri NexusUri { get; private set; }

        private NexusConnectionInfo(Guid accessToken, Uri nexusUri)
        {
            this.AccessToken = accessToken;
            this.NexusUri = nexusUri;
        }

        /// <summary>
        /// Constructs a <see cref="NexusConnectionInfo"/> from the command-line parameters of the process.
        /// </summary>
        /// <param name="error">A variable to hold an error message.</param>
        /// <returns>
        /// an instance of <see cref="NexusConnectionInfo" /> on success, or <c>null</c> on failure.
        /// </returns>
        public static NexusConnectionInfo FromCommandLine(out string error)
        {
            var parameters = ParameterList.FromEnvironment(out error);
            if (error != null)
            {
                error = "Parse error: " + error;
                return null;
            }

            var nexusUriString = parameters[NexusUriKey];

            Uri nexusUri;
            if (nexusUriString == null || !Uri.TryCreate(nexusUriString, UriKind.Absolute, out nexusUri))
            {
                error = "Validation error: " + String.Format(NexusUriParseErrorFormat, NexusUriKey);
                return null;
            }

            var accessTokenString = parameters[AccessTokenKey];

            Guid accessToken;
            if (accessTokenString == null || !Guid.TryParse(accessTokenString, out accessToken))
            {
                error = "Validation error: " + String.Format(TokenParseErrorFormat, AccessTokenKey);
                return null;
            }

            var configuration = new NexusConnectionInfo(accessToken, nexusUri);
            return configuration;
        }
    }
}