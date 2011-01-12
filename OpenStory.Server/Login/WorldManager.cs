using System.Collections.Generic;
using System.Linq;

namespace OpenStory.Server.Login
{
    internal class WorldManager
    {
        private List<World> worlds;

        public WorldManager()
        {
            this.worlds = new List<World>();
        }

        public IWorld GetWorldById(int worldId)
        {
            return this.worlds.First(w => w.Id == worldId);
        }
    }
}