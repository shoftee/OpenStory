using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using OpenMaple.Data;

namespace OpenMaple.Server.Registry
{
    sealed class BuddyList : IEnumerable<BuddyListEntry>
    {
        public const int DefaultCapacity = 20;
        public const int MaxCapacity = 100;

        private Dictionary<int, BuddyListEntry> items;
        private LinkedList<CharacterSimpleInfo> pendingRequests;

        public int Capacity { get; private set; }

        public bool IsFull
        {
            get { return this.items.Count == this.Capacity; }
        }

        private BuddyList(int capacity = DefaultCapacity)
        {
            if (capacity < DefaultCapacity || MaxCapacity < capacity)
            {
                throw new ArgumentOutOfRangeException("capacity");
            }

            this.Capacity = capacity;

            this.items = new Dictionary<int, BuddyListEntry>(capacity);
            this.pendingRequests = new LinkedList<CharacterSimpleInfo>();
        }

        public static BuddyList LoadFromDb(int characterId, int capacity)
        {
            var buddyList = new BuddyList(capacity);
            BuddyListEngine.LoadByCharacterId(characterId, buddyList.HandleRecord);
            return buddyList;
        }

        private void HandleRecord(IDataRecord record) {
            var buddyCharacterId = (int) record["BuddyCharacterId"];
            var buddyName = (string) record["BuddyName"];
            var groupName = (string) record["GroupName"];
            var status = (BuddyListEntryStatus) record["Status"];
            if (status == BuddyListEntryStatus.Pending)
            {
                // TODO: Move this to a better place.
                this.pendingRequests.AddLast(new CharacterSimpleInfo(buddyCharacterId, buddyName));
            }

            var entry = new BuddyListEntry(buddyCharacterId, buddyName, status, groupName);
            this.AddEntry(entry);
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
            return this.items.TryGetValue(characterId, out entry) ? entry : null;
        }

        public void AddEntry(BuddyListEntry entry)
        {
            this.items.Add(entry.CharacterId, entry);
        }

        public bool RemoveEntry(int characterId)
        {
            return this.items.Remove(characterId);
        }

        public IEnumerator<BuddyListEntry> GetEnumerator()
        {
            return this.items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}