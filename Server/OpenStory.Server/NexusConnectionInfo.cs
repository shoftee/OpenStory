using System;
using System.ComponentModel;
using OpenStory.Common.Tools;

namespace OpenStory.Server
{
    /// <summary>
    /// Represents connection information for a nexus service.
    /// </summary>
    [Localizable(true)]
    public sealed class NexusConnectionInfo
    {
        private const string AccessTokenKey = @"AccessToken";
        private const string NexusUriKey = @"NexusUri";

        /// <summary>
        /// Gets the access token required to communicate with the Nexus service.
        /// </summary>
        public Guid AccessToken { get; private set; }

        /// <summary>
        /// Gets the URI of the Nexus service.
        /// </summary>
        public Uri NexusUri { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NexusConnectionInfo"/> class.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="nexusUri">The URI of the nexus service.</param>
        public NexusConnectionInfo(Guid accessToken, Uri nexusUri)
        {
            this.AccessToken = accessToken;
            this.NexusUri = nexusUri;
        }

        /// <summary>
        /// Constructs a <see cref="NexusConnectionInfo"/> from the command-line parameters of the process.
        /// </summary>
        /// <param name="parameters">The parameter list.</param>
        /// <param name="error">A variable to hold an error message.</param>
        /// <returns>
        /// an instance of <see cref="NexusConnectionInfo" /> on success, or <c>null</c> on failure.
        /// </returns>
        public static NexusConnectionInfo FromParameterList(ParameterList parameters, out string error)
        {
            var nexusUriString = parameters[NexusUriKey];

            Uri uri;
            if (nexusUriString == null || !Uri.TryCreate(nexusUriString, UriKind.Absolute, out uri))
            {
                error = string.Format(Errors.BootstrapUriParseError, NexusUriKey);
                return null;
            }

            var accessTokenString = parameters[AccessTokenKey];

            Guid token;
            if (accessTokenString == null || !Guid.TryParse(accessTokenString, out token))
            {
                error = string.Format(Errors.BootstrapTokenParseError, AccessTokenKey);
                return null;
            }

            error = null;
            var result = new NexusConnectionInfo(token, uri);
            return result;
        }
    }
}