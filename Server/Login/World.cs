using System.Collections.Generic;
using OpenMaple.Constants;

namespace OpenMaple.Server.Login
{
    class World : IWorld
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public WorldStatus Status { get; set; }
        public int ChannelCount { get; private set; }

        private List<Channel> channels;
        public IEnumerable<IChannel> Channels { get { return this.channels; } }

        public World(int id, string name, int channelCount)
        {
            this.Id = id;
            this.Name = name;
            this.ChannelCount = channelCount;
            this.channels = new List<Channel>(channelCount);
        }

    }

    public interface IWorld
    {
        int Id { get; }
        string Name { get; }
        WorldStatus Status { get; }
        int ChannelCount { get; }

        IEnumerable<IChannel> Channels { get; }
    }
}