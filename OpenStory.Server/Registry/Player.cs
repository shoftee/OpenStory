using System.Collections.Generic;
using OpenStory.Common;
using OpenStory.Server.Data;
using OpenStory.Server.Game;
using OpenStory.Server.Maps;
using OpenStory.Server.Registry.Buddy;
using OpenStory.Server.Registry.Guild;
using OpenStory.Server.Registry.Messenger;
using OpenStory.Server.Registry.Party;

namespace OpenStory.Server.Registry
{
    sealed partial class Player : IPlayer
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

        private Player(CharacterData characterData)
        {
            // Get what we can from the transfer object.
            this.CharacterId = characterData.Id;
            this.CharacterName = characterData.Name;
            this.WorldId = characterData.WorldId;

            this.Gender = characterData.Gender;
            this.HairId = characterData.HairId;
            this.FaceId = characterData.FaceId;
            this.SkinColor = characterData.SkinColor;

            this.JobId = characterData.JobId;
            this.Fame = characterData.Fame;
            this.Level = characterData.Level;

            // TODO: There are still more things to add to Character

            // Get the rest from the database.
            this.layout = KeyLayout.LoadFromDb(this.CharacterId);

            this.buddyList = BuddyList.LoadFromDb(this.CharacterId, characterData.BuddyListCapacity);
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