using System;

namespace OpenMaple.Server.Maps
{
    [Flags]
    enum FieldLimitFlags
    {
        NoLimit = 0x0,
        Jump = 0x1,
        MovementSkills = 0x2,
        SummoningBag = 0x4,
        MysticDoor = 0x8,
        ChannelChange = 0x10,
        RegularExpLoss = 0x20,
        VipTeleportRock = 0x40,
        Minigames = 0x80,
        // TODO: 0x100
        Mount = 0x200,
        // TODO: 0x400
        // TODO: 0x800
        PotionUse = 0x1000,
        // TODO: 0x2000
        Unused = 0x4000,
        // TODO: 0x8000
        // TODO: 0x10000
        DropDown = 0x20000
    }
}