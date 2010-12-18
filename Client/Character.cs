using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMaple.Client
{
    enum CharacterGender
    {
        None = 0,
        Male,
        Female
    }

    class Character
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public bool IsGameMaster { get; private set; }

        public int Level { get; private set; }
        public int Meso { get; private set; }
        public int Experience { get; private set; }
        public int Fame { get; private set; }

        public int WorldRank { get; private set; }
        public int WorldRankMove { get; private set; }

        public int JobRank { get; private set; }
        public int JobRankMove { get; private set; }

        public int HairId { get; private set; }
        public int FaceId { get; private set; }

        public CharacterGender Gender { get; private set; }
        public int SkinColor { get; private set; }

        public int JobId { get; private set; }
        public int MapId { get; private set; }

        public Client Client { get; private set; }

        public Inventory EquipInventory { get; private set; }
        public Inventory UseInventory { get; private set; }
        public Inventory SetupInventory { get; private set; }
        public Inventory EtcInventory { get; private set; }
        public Inventory CashInventory { get; private set; }

        private KeyLayout layout;

        public BuddyList BuddyList { get; private set; }

        private Character()
        {
        }

        //public static Character GetDefault(Client client, JobClass type)
        //{

        //}
    }

    enum JobClass
    {
        None = 0,
        Adventurer = 1,
        Legend = 2,
        Resistance = 3
    }
}
