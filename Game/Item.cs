using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMaple.Game
{
    class Item : IItem
    {
        public ItemType Type { get; private set; }
        public int UniqueId { get; private set; }
        public int ItemId { get; private set; }
        public bool AllowZero { get; private set; }

        public short Position { get; set; }
        public short Quantity { get; set; }
        public DateTime Expiration { get; set; }

        public Item(int itemId, short position, short quantity, bool allowZero = false)
        {
            this.ItemId = itemId;
            this.AllowZero = allowZero;
            this.Position = position;
            this.Quantity = quantity;
        }
    }
}
