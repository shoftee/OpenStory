namespace OpenStory.Server.Game
{
    internal class Equip : Item, IEquip
    {
        public Equip(int itemId, short position)
            : base(itemId, position, 1) {}

        #region IEquip Members

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

        #endregion
    }
}