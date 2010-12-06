using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenMaple.Client
{
    class ChannelServer
    {
        public const int MaxChannels = 32;

        #region Static shit

        private static readonly Dictionary<int, ChannelServer> InstancesInternal;
        private static readonly Dictionary<string, ChannelServer> PendingInstances;

        public static IEnumerable<IChannelServer> Instances
        {
            get { InstancesInternal.Values.Cast<IChannelServer>(); }
        }

        static ChannelServer()
        {
            InstancesInternal = new Dictionary<int, ChannelServer>(MaxChannels);
            PendingInstances = new Dictionary<string, ChannelServer>(MaxChannels);
        }

        public static ChannelServer New(string key)
        {
            ChannelServer instance = new ChannelServer(key);
            PendingInstances.Add(key, instance);
            return instance;
        }

        #endregion

        public bool IsRunning { get; private set; }

        public PlayerStore Players { get; private set; }
        private string Key { get; set; }
        public int ChannelId { get; private set; }

        private ChannelServer(string key)
        {
            this.Key = key;
        }

        public void AddPlayer(Character character)
        {
            this.Players.RegisterPlayer(character);
            character.Client.Session.Write(MaplePacketCreator.ServerMessage(serverMessage));
        }

        public void RemovePlayer(Character character)
        {
            this.Players.UnregisterPlayer(character);
        }

        public void ShutDown(int time)
        {
            // TODO: Schedule shutdown 
        }

    }
}
