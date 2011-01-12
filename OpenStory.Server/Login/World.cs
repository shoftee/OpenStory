using System.Collections.Generic;

namespace OpenStory.Server.Login
{
    internal class World : IWorld
    {
        private List<Channel> channels;

        public World(int id, string name, int channelCount)
        {
            this.Id = id;
            this.Name = name;
            this.ChannelCount = channelCount;
            this.channels = new List<Channel>(channelCount);
        }

        #region IWorld Members

        public int Id { get; private set; }
        public string Name { get; private set; }
        public WorldStatus Status { get; set; }
        public int ChannelCount { get; private set; }

        public IEnumerable<IChannel> Channels
        {
            get { return this.channels; }
        }

        #endregion
    }
}