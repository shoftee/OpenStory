using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMaple.Game
{
    class Equip : Item, IEquip
    {
        public byte UpgradeSlots { get; private set; }
        public byte ViciousHammers { get; private set; }

        public short Str { get; private set; }
        public short Dex { get; private set; }
        public short Int { get; private set; }
        public short Luk { get; private set; }

        public short Accuracy { get; private set; }
        public short Avoidance { get; private set; }
        public short Hands { get; private set; }
        public short Speed { get; private set; }
        public short Jump { get; private set; }

        public int HealthPoints { get; private set; }
        public int ManaPoints { get; private set; }

        public int MagicAttack { get; private set; }
        public int MagicDefense { get; private set; }
        public int WeaponAttack { get; private set; }
        public int WeaponDefense { get; private set; }

        public byte ItemLevel { get; private set; }
        public short ItemExp { get; private set; }

        public Equip(int itemId, short position)
            : base(itemId, position, 1)
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
