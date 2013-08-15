using System.Collections.Generic;
using System.Linq;
using OpenStory.Common.Game;
using OpenStory.Server.Processing;
using OpenStory.Services;

namespace OpenStory.Server.Auth
{
    internal sealed class AuthOperator : ServerOperator<AuthClient>, IAuthServer
    {
        private readonly List<IWorld> worlds;

        public AuthOperator(IClientFactory<AuthClient> clientFactory)
            : base(clientFactory)
        {
            this.worlds = new List<IWorld>();
        }

        public override void Configure(ServiceConfiguration configuration)
        {
        }

        #region IAuthServer Members

        /// <inheritdoc />
        public IWorld GetWorldById(int worldId)
        {
            return this.worlds.First(w => w.Id == worldId);
        }

        #endregion
    }
}
