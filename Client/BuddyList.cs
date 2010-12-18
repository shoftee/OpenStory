using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using OpenMaple.Tools;

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
        private const string SelectBuddies =
            "SELECT * " +
            "FROM BuddyListEntry " +
            "WHERE [CharacterId]=@characterId";

        public const int DefaultCapacity = 20;
        public const int MaxCapacity = 100;

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
            BuddyList buddyList = new BuddyList(capacity);
            SqlCommand query = new SqlCommand(SelectBuddies);
            query.AddParameter("@characterId", SqlDbType.Int, characterId);
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.OpenMapleConnectionString))
            {
                query.Connection = connection;
                connection.Open();
                using (SqlDataReader reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int buddyCharacterId = (int) reader["BuddyCharacterId"];
                        string buddyName = (string) reader["BuddyName"];
                        string groupName = (string) reader["GroupName"];
                        BuddyListEntryStatus status = (BuddyListEntryStatus) reader["Status"];
                        if (status == BuddyListEntryStatus.PendingRequest)
                        {
                            buddyList.pendingRequests.AddLast(new CharacterSimpleInfo(buddyCharacterId, buddyName));
                        }

                        BuddyListEntry entry = new BuddyListEntry(buddyCharacterId, buddyName, status, groupName);
                        buddyList.AddEntry(entry);
                    }
                }
            }
            return buddyList;
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
        public int Id { get; private set; }
        public string Name { get; private set; }

        public CharacterSimpleInfo(int id, string name)
            : this()
        {
            this.Id = id;
            this.Name = name;
        }
    }

}
