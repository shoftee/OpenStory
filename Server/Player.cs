using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMaple.Game;
using OpenMaple.Server.Maps;
using OpenMaple.Server.Registry;

namespace OpenMaple.Server
{
    partial class Player : IPlayer
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
        private BuddyList buddyList;

        public int Experience { get; private set; }

        private Player(Character character)
        {
            // Get what we can from the transfer object.
            this.CharacterId = character.Id;
            this.CharacterName = character.Name;
            this.WorldId = character.WorldId;

            this.Gender = character.Gender;
            this.HairId = character.HairId;
            this.FaceId = character.FaceId;
            this.SkinColor = character.SkinColor;

            this.JobId = character.JobId;
            this.Fame = character.Fame;
            this.Level = character.Level;

            // TODO: There are still more things to add to Character

            // Get the rest from the database.
            this.layout = KeyLayout.LoadFromDb(this.CharacterId);
            //this.buddyList = BuddyList.LoadFromDb(this.CharacterId, capacity);
        }
    }
}
