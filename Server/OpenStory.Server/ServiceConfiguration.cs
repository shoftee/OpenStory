using System;
using OpenStory.Common.Tools;

namespace OpenStory.Server
{
    /// <summary>
    /// Represents configuration properties needed to initialize a game managed service.
    /// </summary>
    public sealed class ServiceConfiguration
    {
        private const string TokenKey = "Token";
        private const string NexusUriKey = "NexusUri";

        private const string TokenParseErrorFormat =
            "Could not parse access token. Make sure you've entered a valid GUID value under the parameter '{0}' (case-sensitive).";

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

        private ServiceConfiguration(Guid accessToken, Uri nexusUri)
        {
            this.AccessToken = accessToken;
            this.NexusUri = nexusUri;
        }

        /// <summary>
        /// Constructs a service configuration from the command-line parameters of the process.
        /// </summary>
        /// <param name="error">A variable to hold an error message.</param>
        /// <returns>
        /// an instance of <see cref="ServiceConfiguration" /> on success, or <c>null</c> on failure.
        /// </returns>
        public static ServiceConfiguration FromCommandLine(out string error)
        {
            var parameters = ParameterList.FromEnvironment(out error);
            if (error != null)
            {
                error = "Parse error: " + error;
                return null;
            }

            var accessTokenString = parameters[TokenKey];

            Guid accessToken;
            if (!Guid.TryParse(accessTokenString, out accessToken))
            {
                error = String.Format(TokenParseErrorFormat, TokenKey);
                error = "Validation error: " + error;
                return null;
            }

            var nexusUriString = parameters[NexusUriKey];

            Uri nexusUri;
            if (!Uri.TryCreate(nexusUriString, UriKind.Absolute, out nexusUri))
            {
                error = String.Format(NexusUriParseErrorFormat, NexusUriKey);
                error = "Validation error: " + error;
                return null;
            }

            var configuration = new ServiceConfiguration(accessToken, nexusUri);
            return configuration;
        }
    }
}