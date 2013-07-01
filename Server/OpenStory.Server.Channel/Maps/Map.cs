using System;
using System.Collections.Generic;
using System.Linq;
using OpenStory.Common;

namespace OpenStory.Server.Channel.Maps
{
    internal class Map
    {
        private readonly Dictionary<int, IMapObject> mapObjects;
        private readonly Dictionary<string, IPortal> portals;
        private readonly AtomicInteger rollingObjectId;

        private Map()
        {
            this.mapObjects = new Dictionary<int, IMapObject>();
            this.portals = new Dictionary<string, IPortal>();
            this.rollingObjectId = new AtomicInteger(100000);
        }

        public Map(int mapId, int channelId)
            : this()
        {
            this.MapId = mapId;
            this.ChannelId = channelId;
        }

        public int MapId { get; private set; }
        public int ChannelId { get; private set; }

        /// <summary>
        /// Constructs a new object with the given constructor delegate, giving it a proper map object ID.
        /// </summary>
        /// <param name="constructor">An <see cref="Func{Int32, IMapObject}"/>-like delegate used to construct the map object.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="constructor"/> is <c>null</c>.</exception>
        public void AddMapObject(MapObjectConstructor constructor)
        {
            if (constructor == null)
            {
                throw new ArgumentNullException("constructor");
            }

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
            if (!this.mapObjects.TryGetValue(objectId, out mapObject))
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
            if (!this.portals.TryGetValue(portalName, out portal))
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
}
