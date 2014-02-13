using System.Collections.Generic;
using System.Linq;
using OpenStory.Common.Game;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Nexus
{
    internal sealed class NexusServer : RegisteredServiceBase, IAuthToNexusRequestHandler
    {
        private readonly WorldContainer worldContainer;

        public NexusServer(WorldContainer worldContainer)
        {
            this.worldContainer = worldContainer;
        }

        /// <inheritdoc />
        public IEnumerable<IWorld> GetWorlds()
        {
            var services = this.worldContainer.ToList();
            var details = services.Select(w => w.GetDetails()).AsParallel();
            return details.ToList();
        }
    }
}
