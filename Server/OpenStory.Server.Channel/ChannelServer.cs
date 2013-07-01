using System;
using System.Collections.Generic;
using System.Linq;
using OpenStory.Common;
using OpenStory.Framework.Model.Common;
using OpenStory.Server.Modules;

namespace OpenStory.Server.Channel
{
    /// <summary>
    /// Represents a server that handles the channel-side operations.
    /// </summary>
    public sealed class ChannelServer : ServerBase, IChannelServer
    {
        private const string ServerName = @"Channel";

        private readonly List<ChannelClient> clients;

        /// <inheritdoc />
        public override string Name
        {
            get { return ServerName; }
        }

        /// <inheritdoc />
        protected override IOpCodeTable OpCodes
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the channel identifier.
        /// </summary>
        public int ChannelId { get; private set; }

        /// <summary>
        /// Gets the world identifier.
        /// </summary>
        public int WorldId { get; private set; }

        /// <inheritdoc />
        public IChannelWorld World { get; private set; }

        /// <inheritdoc />
        public ChannelServer(ChannelConfiguration configuration)
            : base(configuration)
        {
            this.ChannelId = configuration.ChannelId;
            this.WorldId = configuration.WorldId;

            this.clients = new List<ChannelClient>();
        }

        /// <inheritdoc />
        protected override void OnConnectionOpen(IServerSession serverSession)
        {
            var newClient = new ChannelClient(this, serverSession);
            this.clients.Add(newClient);
        }

        /// <inheritdoc />
        public void BroadcastToWorld(CharacterKey sourceKey, IEnumerable<CharacterKey> targets, byte[] data)
        {
            var ids = from id in targets
                      where id != sourceKey
                      select id;

            this.BroadcastToWorld(ids, data);
        }

        private void BroadcastToWorld(IEnumerable<CharacterKey> targets, byte[] data)
        {
            // Arrays are more efficient for remoting operations.
            CharacterKey[] keys = targets.ToArray();

            var playerRegistry = LookupManager.GetManager().Players;
            this.BroadcastIntoChannel(playerRegistry.Scan(keys).Select(p => p.Key), data);

            this.World.BroadcastFromChannel(this.ChannelId, keys, data);
        }

        // This method will be part of the service contract.
        /// <summary>
        /// Broadcasts a packet to the targets that reside in the current channel.
        /// </summary>
        /// <param name="targets">The characters to broadcast to.</param>
        /// <param name="data">The packet payload to broadcast.</param>
        public void BroadcastIntoChannel(IEnumerable<CharacterKey> targets, byte[] data)
        {
            var playerRegistry = LookupManager.GetManager().Players;
            var targetPlayers = from target in playerRegistry.Scan(targets)
                                select target;

            foreach (var player in targetPlayers)
            {
                player.Client.WritePacket(data);
            }
        }
    }
}
