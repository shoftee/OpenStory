using System;
using System.Collections.Generic;
using System.Linq;
using OpenStory.Server.Registry;

namespace OpenStory.Server
{
    internal sealed class ChannelServer
    {
        public const int MaxChannels = 16;

        #region Static shit

        private static readonly Dictionary<int, ChannelServer> InstancesInternal;
        private static readonly Dictionary<string, ChannelServer> PendingInstances;

        static ChannelServer()
        {
            InstancesInternal = new Dictionary<int, ChannelServer>(MaxChannels);
            PendingInstances = new Dictionary<string, ChannelServer>(MaxChannels);
        }

        public static IEnumerable<IChannelServer> Instances
        {
            get { return InstancesInternal.Values.Cast<IChannelServer>(); }
        }

        #endregion

        private ChannelServer(int channelId)
        {
            this.ChannelId = channelId;
        }

        public bool IsRunning { get; private set; }

        public PlayerManager Players { get; private set; }
        public int ChannelId { get; private set; }

        public int ExpRate { get; private set; }
        public int DropRate { get; private set; }
        public int MesoRate { get; private set; }

        public void AddPlayer(Player player)
        {
            this.Players.RegisterPlayer(player);
        }

        public void RemovePlayer(Player player)
        {
            this.Players.UnregisterPlayer(player);
        }

        public void ShutDown(TimeSpan time)
        {
            // TODO: Schedule shutdown 
        }
    }

    internal interface IChannelServer {}
}