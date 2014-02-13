using System.Collections;
using System.Collections.Generic;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Nexus
{
    internal sealed class WorldContainer : IServiceContainer<INexusToWorldRequestHandler>, IEnumerable<INexusToWorldRequestHandler>
    {
        private readonly Dictionary<int, INexusToWorldRequestHandler> worlds;

        public WorldContainer()
        {
            this.worlds = new Dictionary<int, INexusToWorldRequestHandler>();
        }

        /// <inheritdoc />
        public void Register(INexusToWorldRequestHandler world)
        {
            this.worlds.Add(world.WorldId, world);
        }

        /// <inheritdoc />
        public void Unregister(INexusToWorldRequestHandler world)
        {
            this.worlds.Remove(world.WorldId);
        }

        public IEnumerator<INexusToWorldRequestHandler> GetEnumerator()
        {
            return this.worlds.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
