using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OpenStory.Server.Game
{
    internal class Inventory : IEnumerable<IItem>
    {
        public const int MinCapacity = 24;
        public const int MaxCapacity = 96;

        private Dictionary<short, IItem> items;

        public Inventory(InventoryType type, int capacity = MinCapacity)
        {
            if (!(MinCapacity <= capacity && capacity <= MaxCapacity))
            {
                throw new ArgumentOutOfRangeException("capacity");
            }
            this.InventoryType = type;
            this.Capacity = capacity;
            this.items = new Dictionary<short, IItem>(capacity);
        }

        public int Capacity { get; private set; }
        public InventoryType InventoryType { get; private set; }

        public bool IsFull
        {
            get { return this.items.Count == this.Capacity; }
        }

        public int FreeSlotCount
        {
            get { return this.Capacity - this.items.Count; }
        }

        public IItem FindById(int itemId)
        {
            return this.FirstOrDefault(item => item.ItemId == itemId);
        }

        public int CountById(int itemId)
        {
            return this.Sum(item => item.Quantity);
        }

        public IEnumerable<IItem> ListById(int itemId)
        {
            return this.Where(item => item.ItemId == itemId);
        }

        public short AddItem(IItem item)
        {
            if (item == null) throw new ArgumentNullException("item");

            short slotId = this.GetFreeSlot();
            if (slotId < 0)
            {
                return -1;
            }
            this.items.Add(slotId, item);
            // TODO: Check where this Position property is used.
            item.Position = slotId;
            return slotId;
        }

        public void RemoveItem(short slot, int quantity)
        {
            IItem item;
            if (!this.items.TryGetValue(slot, out item))
            {
                throw GetSlotIsEmptyException(slot);
            }
            if (item.Quantity < quantity)
            {
                throw new InvalidOperationException("'quantity' is bigger than the number of items in the slot.");
            }

            item.Quantity -= (short) quantity;
            if (item.Quantity == 0 && !item.AllowZero)
            {
                this.items.Remove(slot);
            }
        }

        public void RemoveSingleItem(short slot)
        {
            IItem item;
            if (!this.items.TryGetValue(slot, out item))
            {
                throw GetSlotIsEmptyException(slot);
            }

            item.Quantity--;
            if (item.Quantity == 0 && !item.AllowZero)
            {
                this.items.Remove(slot);
            }
        }

        private short GetFreeSlot()
        {
            if (this.IsFull)
            {
                return -1;
            }
            for (short i = 1; i <= this.Capacity; i++)
            {
                if (!this.items.ContainsKey(i))
                {
                    return i;
                }
            }

            // We should never reach this point, so if we do something is very wrong.
            throw new Exception("Something very fucked up happened.");
        }

        private static InvalidOperationException GetSlotIsEmptyException(short slot)
        {
            return new InvalidOperationException("Slot " + slot + " is empty.");
        }

        #region IEnumerable<IItem> methods

        public IEnumerator<IItem> GetEnumerator()
        {
            return this.items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}