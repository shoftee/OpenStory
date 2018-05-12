using System.Collections;
using System.Collections.Generic;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Nexus
{
    internal sealed class WorldContainer : IServiceContainer<INexusToWorldRequestHandler>, IEnumerable<INexusToWorldRequestHandler>
    {
        private readonly Dictionary<int, INexusToWorldRequestHandler> _worlds;

        public WorldContainer()
        {
            _worlds = new Dictionary<int, INexusToWorldRequestHandler>();
        }

        /// <inheritdoc />
        public void Register(INexusToWorldRequestHandler world)
        {
            _worlds.Add(world.WorldId, world);
        }

        /// <inheritdoc />
        public void Unregister(INexusToWorldRequestHandler world)
        {
            _worlds.Remove(world.WorldId);
        }

        public IEnumerator<INexusToWorldRequestHandler> GetEnumerator()
        {
            return _worlds.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
