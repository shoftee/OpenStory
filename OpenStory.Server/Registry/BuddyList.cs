using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using OpenStory.Server.Data;

namespace OpenStory.Server.Registry
{
    internal sealed class BuddyList : IEnumerable<Buddy>
    {
        public const int DefaultCapacity = 20;
        public const int MaxCapacity = 100;

        private Dictionary<int, Buddy> items;
        private LinkedList<CharacterSimpleInfo> pendingRequests;

        private BuddyList(int capacity = DefaultCapacity)
        {
            this.Capacity = capacity;

            this.items = new Dictionary<int, Buddy>(capacity);
            this.pendingRequests = new LinkedList<CharacterSimpleInfo>();
        }

        public int Capacity { get; private set; }

        public bool IsFull
        {
            get { return this.items.Count == this.Capacity; }
        }

        #region IEnumerable<Buddy> Members

        public IEnumerator<Buddy> GetEnumerator()
        {
            return this.items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        public static BuddyList LoadFromDb(int characterId, int capacity)
        {
            var buddyList = new BuddyList(capacity);
            BuddyListEngine.LoadByCharacterId(characterId, buddyList.HandleRecord);
            return buddyList;
        }

        private void HandleRecord(IDataRecord record)
        {
            var buddyCharacterId = (int) record["BuddyCharacterId"];
            var buddyName = (string) record["BuddyName"];
            var groupName = (string) record["GroupName"];
            var status = (BuddyStatus) record["Status"];
            if (status == BuddyStatus.Pending)
            {
                // TODO: Move this to a better place.
                this.pendingRequests.AddLast(new CharacterSimpleInfo(buddyCharacterId, buddyName));
            }

            var entry = new Buddy(buddyCharacterId, buddyName, status, groupName);
            this.AddEntry(entry);
        }

        public bool ContainsId(int characterId)
        {
            return this.items.ContainsKey(characterId);
        }

        public bool TryGetById(int characterId, out Buddy entry)
        {
            return this.items.TryGetValue(characterId, out entry);
        }

        public Buddy GetById(int characterId)
        {
            Buddy entry;
            return this.items.TryGetValue(characterId, out entry) ? entry : null;
        }

        public void AddEntry(Buddy entry)
        {
            this.items.Add(entry.CharacterId, entry);
        }

        public bool RemoveEntry(int characterId)
        {
            return this.items.Remove(characterId);
        }
    }
}