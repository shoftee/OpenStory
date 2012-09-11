using System;
using System.Collections.Generic;
using System.Linq;
using OpenStory.Common.Data;

namespace OpenStory.Server.Channel
{
    /// <summary>
    /// Represents a server that handles the channel-side operations.
    /// </summary>
    public sealed class ChannelServer : ServerBase, IChannelServer
    {
        private readonly int channelId = 0; // TODO

        private readonly List<ChannelClient> clients;
        private readonly PlayerRegistry players;

        /// <inheritdoc />
        public override string Name
        {
            get { return "Channel"; }
        }

        /// <inheritdoc />
        public override IOpCodeTable OpCodes
        {
            get { throw new NotImplementedException(); }
        }

        /// <inheritdoc />
        public IChannelWorld World { get; private set; }

        /// <inheritdoc />
        public ChannelServer(ServerConfiguration configuration)
            : base(configuration)
        {
            this.clients = new List<ChannelClient>();
            this.players = new PlayerRegistry();
        }

        /// <inheritdoc />
        protected override void OnConnectionOpen(ServerSession serverSession)
        {
            var newClient = new ChannelClient(this, serverSession);
            this.clients.Add(newClient); // NOTE: Happens both in Auth and Channel servers, pull up?
        }

        /// <inheritdoc />
        public void BroadcastToWorld(int sourceId, IEnumerable<int> targetIds, byte[] data)
        {
            var ids = from id in targetIds
                      where id != sourceId
                      select id;

            this.BroadcastToWorld(ids, data);
        }

        private void BroadcastToWorld(IEnumerable<int> targetIds, byte[] data)
        {
            // Arrays are more efficient for remoting operations.
            int[] ids = targetIds.ToArray();

            this.BroadcastIntoChannel(players.GetActive(ids), data);

            this.World.BroadcastFromChannel(this.channelId, ids, data);
        }

        // This method will be part of the service contract.
        /// <summary>
        /// Broadcasts a packet to the targets that reside in the current channel.
        /// </summary>
        /// <param name="targetIds"></param>
        /// <param name="data"></param>
        public void BroadcastIntoChannel(IEnumerable<int> targetIds, byte[] data)
        {
            var targets = from targetId in targetIds
                          select this.players.GetById(targetId) into target
                          where target != null
                          select target;

            foreach (var player in targets)
            {
                player.Client.WritePacket(data);
            }
        }
    }
}
