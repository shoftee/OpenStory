using System.Collections.Generic;

namespace OpenMaple.Server.Login
{
    internal class World : IWorld
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public WorldStatus Status { get; set; }
        public int ChannelCount { get; private set; }

        private List<Channel> channels;

        public IEnumerable<IChannel> Channels
        {
            get { return this.channels; }
        }

        public World(int id, string name, int channelCount)
        {
            this.Id = id;
            this.Name = name;
            this.ChannelCount = channelCount;
            this.channels = new List<Channel>(channelCount);
        }
    }
}