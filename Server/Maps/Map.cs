using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenMaple.Game;
using OpenMaple.Threading;

namespace OpenMaple.Server.Maps
{
    internal delegate IMapObject MapObjectConstructor(int mapObjectId);

    class Map
    {
        private readonly Dictionary<int, IMapObject> mapObjects;
        private readonly Dictionary<string, IPortal> portals;
        private readonly AtomicInteger rollingObjectId;

        public int MapId { get; private set; }
        public int ChannelId { get; private set; }

        private Map()
        {
            this.mapObjects = new Dictionary<int, IMapObject>();
            this.portals = new Dictionary<string, IPortal>();
            this.rollingObjectId = new AtomicInteger(100000);
        }

        public Map(int mapId, int channelId) : this()
        {
            this.MapId = mapId;
            this.ChannelId = channelId;
        }

        /// <summary>
        /// Constructs a new object with the given constructor delegate, giving it a proper map object ID.
        /// </summary>
        /// <param name="constructor">An (int) => (IMapObject) delegate used to construct the map object.</param>
        /// <exception cref="ArgumentNullException">The exception is thrown if <paramref name="constructor"/> is null.</exception>
        public void AddMapObject(MapObjectConstructor constructor)
        {
            if (constructor == null) throw new ArgumentNullException("constructor");

            // Again, quite proud of this idea, it looks so cool o.o
            int mapObjectId = this.rollingObjectId.Increment();
            IMapObject newObject = constructor(mapObjectId);
            this.mapObjects.Add(mapObjectId, newObject);
        }

        public void RemoveMapObject(int objectId)
        {
            this.mapObjects.Remove(objectId);
        }

        public IMapObject GetMapObject(int objectId)
        {
            IMapObject mapObject;
            if (this.mapObjects.TryGetValue(objectId, out mapObject))
            {
                return null;
            }
            return mapObject;
        }

        public void AddPortal(IPortal portal)
        {
            this.portals.Add(portal.Name, portal);
        }

        public IPortal GetPortalByName(string portalName)
        {
            IPortal portal;
            if (this.portals.TryGetValue(portalName, out portal))
            {
                return null;
            }
            return portal;
        }

        public IPortal GetPortalById(int id)
        {
            return this.portals.Values.FirstOrDefault(p => p.Id == id);
        }
    }

    [Flags]
    enum FieldLimitFlags
    {
        NoLimit = 0x0,
        Jump = 0x1,
        MovementSkills = 0x2,
        SummoningBag = 0x4,
        MysticDoor = 0x8,
        ChannelChange = 0x10,
        RegularExpLoss = 0x20,
        VipTeleportRock = 0x40,
        Minigames = 0x80,
        // TODO: 0x100
        Mount = 0x200,
        // TODO: 0x400
        // TODO: 0x800
        PotionUse = 0x1000,
        // TODO: 0x2000
        Unused = 0x4000,
        // TODO: 0x8000
        // TODO: 0x10000
        DropDown = 0x20000
    }
}
