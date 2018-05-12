using System.Collections.Generic;
using System.Linq;
using OpenStory.Common.Game;
using OpenStory.Server.Processing;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Nexus
{
    internal sealed class NexusServer : GameServerBase, IAuthToNexusRequestHandler
    {
        private readonly WorldContainer _worldContainer;

        public NexusServer(WorldContainer worldContainer)
        {
            _worldContainer = worldContainer;
        }

        /// <inheritdoc />
        public IEnumerable<IWorld> GetWorlds()
        {
            var services = _worldContainer.ToList();
            var details = services.Select(w => w.GetDetails()).AsParallel();
            return details.ToList();
        }
    }
}
