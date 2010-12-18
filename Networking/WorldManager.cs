using System.Collections.Generic;

namespace OpenMaple.Networking
{
    class WorldManager
    {
        private Dictionary<int, World> worlds;

        public WorldManager()
        {
            worlds = new Dictionary<int, World>();
        }
    }
}