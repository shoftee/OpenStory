using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMaple.Constants;

namespace OpenMaple.Game
{
    class Item : IItem
    {
        public ItemType Type { get; set; }
        public byte Flag { get; set; }
        public short Position { get; set; }
        public short Quantity { get; set; }
        public int ItemId { get; set; }
        public int UniqueId { get; set; }
        public DateTime Expiration { get; set; }

        public Item(int itemId, short position, short quantity, byte flag)
        {
            this.ItemId = itemId;
            this.Position = position;
            this.Quantity = quantity;
            this.Flag = flag;
        }
    }
}
