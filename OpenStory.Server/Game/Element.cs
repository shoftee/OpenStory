using System;

namespace OpenStory.Server.Game
{
    [Flags]
    public enum Element
    {
        None = 0,
        Fire = 0x1, // F
        Ice = 0x2, // I
        Ligtning = 0x4, // L
        Poison = 0x8, // S
        Holy = 0x10, // H
        Dark = 0x20, // D
        Physical = 0x40 // P
    }
}