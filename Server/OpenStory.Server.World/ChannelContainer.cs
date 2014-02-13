using System.Collections;
using System.Collections.Generic;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.World
{
    internal sealed class ChannelContainer : IServiceContainer<IWorldToChannelRequestHandler>, IEnumerable<IWorldToChannelRequestHandler>
    {
        private readonly Dictionary<int, IWorldToChannelRequestHandler> channels;

        public ChannelContainer()
        {
            this.channels = new Dictionary<int, IWorldToChannelRequestHandler>();
        }

        /// <inheritdoc />
        public void Register(IWorldToChannelRequestHandler channel)
        {
            this.channels.Add(channel.ChannelId, channel);
        }

        /// <inheritdoc />
        public void Unregister(IWorldToChannelRequestHandler channel)
        {
            this.channels.Remove(channel.ChannelId);
        }

        public IEnumerator<IWorldToChannelRequestHandler> GetEnumerator()
        {
            return this.channels.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
