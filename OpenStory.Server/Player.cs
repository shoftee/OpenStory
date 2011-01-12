using System.Collections.Generic;
using OpenStory.Common;
using OpenStory.Server.Game;
using OpenStory.Server.Maps;
using OpenStory.Server.Registry;

namespace OpenStory.Server
{
    internal partial class Player : IPlayer
    {
        #region Visible info

        public int WorldId { get; private set; }

        public Gender Gender { get; private set; }
        public int HairId { get; private set; }
        public int FaceId { get; private set; }
        public int SkinColor { get; private set; }

        public int Fame { get; private set; }

        public MessengerMember MessengerMember { get; private set; }
        public PartyMember PartyMember { get; private set; }
        public GuildMember GuildMember { get; private set; }
        public int CharacterId { get; private set; }
        public string CharacterName { get; private set; }
        public int JobId { get; private set; }
        public int Level { get; private set; }

        public int ChannelId { get; private set; }
        public int MapId { get; private set; }

        #endregion

        private BuddyList buddyList;
        private KeyLayout layout;
        private Dictionary<int, Skill> skills;

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

        public Map Map { get; private set; }

        public int Meso { get; private set; }
        public Inventory EquipInventory { get; private set; }
        public Inventory UseInventory { get; private set; }
        public Inventory SetupInventory { get; private set; }
        public Inventory EtcInventory { get; private set; }
        public Inventory CashInventory { get; private set; }

        public int Experience { get; private set; }
    }
}