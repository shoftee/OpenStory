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
    public sealed class AuthOperator : ServerOperator<AuthClient>
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
        public override void Configure(OsServiceConfiguration configuration)
        {
            this.authConfiguration = new AuthConfiguration(configuration);
            this.SetUp();
        }

        private void SetUp()
        {
        }

        #region IAuthOperator Members

        /// <summary>
        /// Gets a <see cref="IWorld"/> instance by the World's ID.
        /// </summary>
        /// <param name="worldId">The ID of the world.</param>
        /// <returns>an <see cref="IWorld"/> object which represents the world with the given ID.</returns>
        public IWorld GetWorldById(int worldId)
        {
            return this.worlds.First(w => w.Id == worldId);
        }

        #endregion
    }
}
