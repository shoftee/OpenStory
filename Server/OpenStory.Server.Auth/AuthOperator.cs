using System.Collections.Generic;
using System.Linq;
using OpenStory.Common.Game;
using OpenStory.Server.Processing;
using OpenStory.Services;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Auth
{
    /// <summary>
    /// Represents an authentication server operator.
    /// </summary>
    public sealed class AuthOperator : ServerOperator<AuthClient>, IAuthOperator
    {
        private readonly List<IWorld> worlds;

        private AuthConfiguration authConfiguration;

        /// <inheritdoc />
        public AuthOperator(IGameClientFactory<AuthClient> gameClientFactory)
            : base(gameClientFactory)
        {
            this.worlds = new List<IWorld>();
        }

        /// <inheritdoc />
        public override void Configure(ServiceConfiguration configuration)
        {
            this.authConfiguration = new AuthConfiguration(configuration);
            this.SetUp();
        }

        private void SetUp()
        {
        }

        #region IAuthOperator Members

        /// <inheritdoc />
        public IWorld GetWorldById(int worldId)
        {
            return this.worlds.First(w => w.Id == worldId);
        }

        #endregion
    }
}
