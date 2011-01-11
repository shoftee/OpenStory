namespace OpenMaple.Game
{
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