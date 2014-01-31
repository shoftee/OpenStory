using System.Collections.Generic;
using System.Linq;
using OpenStory.Common.Game;
using OpenStory.Server.Processing;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Auth
{
    /// <summary>
    /// Represents an authentication server operator.
    /// </summary>
    public sealed class AuthOperator : ServerOperator<AuthClient>
    {
        private AuthConfiguration authConfiguration;

        /// <inheritdoc />
        public AuthOperator(IGameClientFactory<AuthClient> gameClientFactory)
            : base(gameClientFactory)
        {
        }

        /// <inheritdoc />
        public override void Configure(OsServiceConfiguration configuration)
        {
            this.authConfiguration = new AuthConfiguration(configuration);
        }
    }
}
