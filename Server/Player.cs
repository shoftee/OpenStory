using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMaple.Constants;
using OpenMaple.Game;
using OpenMaple.Server.Maps;
using OpenMaple.Server.Registry;

namespace OpenMaple.Server
{
    class Player : IPlayer
    {
        #region Visible info

        public int CharacterId { get; private set; }
        public string CharacterName { get; private set; }
        public int WorldId { get; private set; }
        public Gender Gender { get; private set; }

        public int HairId { get; private set; }
        public int FaceId { get; private set; }
        public int SkinColor { get; private set; }

        public int JobId { get; private set; }
        public int Fame { get; private set; }
        public int Level { get; private set; }

        public int ChannelId { get; private set; }
        public int MapId { get; private set; }

        public MessengerMember MessengerMember { get; private set; }
        public PartyMember PartyMember { get; private set; }
        public GuildMember GuildMember { get; private set; }

        #endregion

        public Map Map { get; private set; }

        public int Meso { get; private set; }
        public Inventory EquipInventory { get; private set; }
        public Inventory UseInventory { get; private set; }
        public Inventory SetupInventory { get; private set; }
        public Inventory EtcInventory { get; private set; }
        public Inventory CashInventory { get; private set; }

        private Dictionary<int, Skill> skills;
        private KeyLayout layout;

        public BuddyList BuddyList { get; private set; }

        public int Experience { get; private set; }
    }

    interface IPlayer
    {
        int CharacterId { get; }
        string CharacterName { get; }
        int ChannelId { get; }
        int JobId { get; }
        int Level { get; }
        int MapId { get; }
    }
}
