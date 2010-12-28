using System;
using System.Collections.Generic;
using System.Drawing;
using OpenMaple.Game;
using OpenMaple.Tools;

namespace OpenMaple.Server.Maps
{
    class Map
    {
        private readonly Dictionary<int, IMapObject> mapObjects;
        private readonly List<ISpawnPoint> monsterSpawnPoints;
        private int spawnedMonstersCount;
        private readonly List<Character> players;
        private readonly Dictionary<int, IPortal> portals;
        private readonly List<Rectangle> areas;
        private readonly AtomicInteger runningObjectId;
        private FootholdTree footholds;
        private float monsterRate, recoveryRate;
        private MapEffect mapEffect;

        public int MapId { get; private set; }

        public string StreetName { get; private set; }
        public string MapName { get; private set; }

        public int ReturnMapId { get; private set; }
        public FieldLimitFlags FieldLimitFlags { get; private set; }

        public int ChannelId { get; private set; }
        public bool IsTownMap { get; private set; }
        public bool HasTimeLimit { get; private set; }
        public bool AllowPersonalShop { get; private set; }
        public bool DropsEnabled { get; private set; }

        private Map()
        {
            this.mapObjects = new Dictionary<int, IMapObject>();
            this.monsterSpawnPoints = new List<ISpawnPoint>();
            this.players = new List<Character>();
            this.portals = new Dictionary<int, IPortal>();
            this.areas = new List<Rectangle>();
            this.mapEffect = new MapEffect();
            this.runningObjectId = new AtomicInteger(100000);
            this.footholds = new FootholdTree();
        }

        public Map(int mapId, int channelId) : this()
        {
            this.MapId = mapId;
            this.ChannelId = channelId;
        }

        public void AddMapObject(IMapObject mapObject)
        {
            
        }
    }

    [Flags]
    enum FieldLimitFlags
    {
        NoLimit = 0
    }
}
