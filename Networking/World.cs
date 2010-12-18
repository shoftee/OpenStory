using System.Collections.Generic;

namespace OpenMaple.Networking
{
    class World
    {
        public byte Id { get; private set; }
        public string Name { get; private set; }
        public WorldStatus Status { get; set; }
        public byte ChannelCount { get; private set; }
        private List<Channel> channels;

        public World(byte id, string name, byte channelCount)
        {
            this.Id = id;
            this.Name = name;
            this.ChannelCount = channelCount;
            this.channels = new List<Channel>(channelCount);
        }

    }

    enum WorldStatus
    {
        Normal = 0,
        HighlyPopulated = 1,
        Full = 2
    }
}