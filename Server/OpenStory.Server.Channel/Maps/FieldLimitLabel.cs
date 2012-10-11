using System;

namespace OpenStory.Server.Channel.Maps
{
    internal enum FieldLimitLabel
    {
        Jump = 0,//0x1,
        MovementSkills = 1,//0x2,
        SummoningBag = 2,//0x4,
        MysticDoor = 3,//0x8,
        ChannelChange = 4,//0x10,
        RegularExpLoss = 5,//0x20,
        VipTeleportRock = 6,//0x40,
        Minigames = 7,//0x80,
        // TODO: 8,//0x100
        Mount = 9,//0x200,
        // TODO: 10,//0x400
        // TODO: 11,//0x800
        PotionUse = 12,//0x1000,
        // TODO: 13,//0x2000
        Unused = 14,//0x4000,
        // TODO: 15,//0x8000
        // TODO: 16,//0x10000
        DropDown = 17,//0x20000
    }
}
