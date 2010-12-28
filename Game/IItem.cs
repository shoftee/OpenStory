using System;
using OpenMaple.Constants;

namespace OpenMaple.Game
{
    interface IItem
    {
        ItemType Type { get; set; }
        byte Flag { get; set; }
        short Position { get; set; }
        short Quantity { get; set; }
        int ItemId { get; set; }
        int UniqueId { get; set; }
        DateTime Expiration { get; set; }
    }
}