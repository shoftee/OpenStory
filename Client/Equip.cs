using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMaple.Client
{
    class Equip : Item, IEquip
    {
        public byte UpgradeSlots { get; set; }
        public byte ViciousHammers { get; set; }

        public short Str { get; set; }
        public short Dex { get; set; }
        public short Int { get; set; }
        public short Luk { get; set; }

        public short Accuracy { get; set; }
        public short Avoidance { get; set; }
        public short Hands { get; set; }
        public short Speed { get; set; }
        public short Jump { get; set; }

        public int HealthPoints { get; set; }
        public int ManaPoints { get; set; }

        public int MagicAttack { get; set; }
        public int MagicDefense { get; set; }
        public int WeaponAttack { get; set; }
        public int WeaponDefense { get; set; }

        public byte ItemLevel { get; set; }
        public short ItemExp { get; set; }

        public Equip(int itemId, short position, byte flag)
            : base(itemId, position, 1, flag)
        {
        }

    }

    internal interface IEquip
    {
        byte UpgradeSlots { get; }
        byte ViciousHammers { get; }

        short Str { get; }
        short Dex { get; }
        short Int { get; }
        short Luk { get; }

        short Accuracy { get; }
        short Avoidance { get; }
        short Hands { get; }
        short Speed { get; }
        short Jump { get; }

        int HealthPoints { get; }
        int ManaPoints { get; }

        int MagicAttack { get; }
        int MagicDefense { get; }
        int WeaponAttack { get; }
        int WeaponDefense { get; }

        byte ItemLevel { get; }
        short ItemExp { get; }
    }
}
