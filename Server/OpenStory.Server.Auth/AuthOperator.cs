using OpenStory.Server.Processing;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Auth
{
    /// <summary>
    /// Represents an authentication server operator.
    /// </summary>
    public sealed class AuthOperator : ServerOperator<AuthClient>
    {
        private AuthConfiguration _authConfiguration;

        /// <inheritdoc />
        public AuthOperator(IGameClientFactory<AuthClient> gameClientFactory)
            : base(gameClientFactory)
        {
        }

        /// <inheritdoc />
        public override void Configure(OsServiceConfiguration configuration)
        {
            _authConfiguration = new AuthConfiguration(configuration);
        }
    }
}
