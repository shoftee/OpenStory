using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMaple.Client
{
    public enum BuddyListOperation
    {
        None = 0,
        AddBuddy,
        RemoveBuddy
    }

    public enum AddBuddyResult
    {
        None = 0,
        BuddyListFull,
        AlreadyOnList,
        Success
    }

    class BuddyList
    {
        private IDictionary<int, BuddyListEntry> items;
        private LinkedList<CharacterSimpleInfo> pendingRequests;

        public int Capacity { get; private set; }
        public bool IsFull
        {
            get { return items.Count == this.Capacity; }
        }
        public IEnumerable<BuddyListEntry> Buddies
        {
            get { return items.Values; }
        }

        public BuddyList()
        {
            this.Capacity = 20;
            this.items = new Dictionary<int, BuddyListEntry>();
            this.pendingRequests = new LinkedList<CharacterSimpleInfo>();
        }

        public bool ContainsId(int characterId)
        {
            return this.items.ContainsKey(characterId);
        }

        public bool TryGetById(int characterId, out BuddyListEntry entry)
        {
            return this.items.TryGetValue(characterId, out entry);
        }

        public BuddyListEntry GetById(int characterId)
        {
            BuddyListEntry entry;
            if (this.items.TryGetValue(characterId, out entry))
            {
                return entry;
            }
            return null;
        }

        public int[] GetBuddyIds()
        {
            return items.Keys.ToArray();
        }

        public BuddyListEntry GetByName(string characterName)
        {
            return items.Values.FirstOrDefault(
                entry => String.Equals(entry.Name, characterName, StringComparison.OrdinalIgnoreCase)
            );
        }

        public void AddEntry(BuddyListEntry entry)
        {
            items.Add(entry.CharacterId, entry);
        }

        public bool RemoveEntry(int characterId)
        {
            return items.Remove(characterId);
        }
    }

    struct CharacterSimpleInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
