using System;
using System.Collections.Generic;
using System.Linq;
using OpenMaple.Client;
using OpenMaple.Handling.World;

namespace OpenMaple.Networking
{
    sealed class ChannelServer
    {
        public const int MaxChannels = 16;

        #region Static shit

        private static readonly Dictionary<int, ChannelServer> InstancesInternal;
        private static readonly Dictionary<string, ChannelServer> PendingInstances;

        public static IEnumerable<IChannelServer> Instances
        {
            get { return InstancesInternal.Values.Cast<IChannelServer>(); }
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

        public CharacterManager Characters { get; private set; }
        public int ChannelId { get; private set; }

        public int ExpRate { get; private set; }
        public int DropRate { get; private set; }
        public int MesoRate { get; private set; }

        private string Key { get; set; }

        private ChannelServer(string key)
        {
            this.Key = key;
        }

        public void AddPlayer(Character character)
        {
            this.Characters.RegisterPlayer(character);
            // TODO: For later :P
            //Character.Client.Session.Write(MaplePacketCreator.ServerMessage(serverMessage));
        }

        public void RemovePlayer(Character character)
        {
            this.Characters.UnregisterPlayer(character);
        }

        public void ShutDown(TimeSpan time)
        {
            // TODO: Schedule shutdown 
        }

    }

    interface IChannelServer
    {
        
    }
}
