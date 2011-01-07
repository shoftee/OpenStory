using System;
using OpenMaple.Constants;

namespace OpenMaple.Game
{
    interface IItem
    {
        ItemType Type { get; }
        short Position { get; set; }
        short Quantity { get; set; }
        int ItemId { get; }
        int UniqueId { get; }

        bool AllowZero { get; }
        DateTime Expiration { get; set; }
    }
}