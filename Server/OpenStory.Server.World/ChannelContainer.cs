using System.Collections;
using System.Collections.Generic;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.World
{
    internal sealed class ChannelContainer : IServiceContainer<IWorldToChannelRequestHandler>, IEnumerable<IWorldToChannelRequestHandler>
    {
        private readonly Dictionary<int, IWorldToChannelRequestHandler> _channels;

        public ChannelContainer()
        {
            _channels = new Dictionary<int, IWorldToChannelRequestHandler>();
        }

        /// <inheritdoc />
        public void Register(IWorldToChannelRequestHandler channel)
        {
            _channels.Add(channel.ChannelId, channel);
        }

        /// <inheritdoc />
        public void Unregister(IWorldToChannelRequestHandler channel)
        {
            _channels.Remove(channel.ChannelId);
        }

        public IEnumerator<IWorldToChannelRequestHandler> GetEnumerator()
        {
            return _channels.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
