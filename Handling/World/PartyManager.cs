using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenMaple.Client;

namespace OpenMaple.Handling.World
{
    sealed class PartyManager
    {
        private static readonly PartyManager Instance = new PartyManager();
        private int rollingPartyId;

        private ConcurrentDictionary<int, Party> parties;

        private PartyManager()
        {
            this.parties = new ConcurrentDictionary<int, Party>();
        }

        public static IParty CreateParty(Character leaderCharacter)
        {
            if (leaderCharacter == null) throw new ArgumentNullException("leaderCharacter");
            PartyMember leader = new PartyMember(leaderCharacter);
            // TODO: Check if he's in a party already o.O
            int newPartyId = GetNewId();
            Party party = new Party(newPartyId, leader);
            if (!Instance.parties.TryAdd(newPartyId, party))
            {
                throw new Exception("what.");
            }
            return party;
        }

        public static IParty GetPartyById(int partyId)
        {
            // NOTE: Persisting the party across sessions.
            // The party needs to stay the same even if all members are offline.

            Party party;
            if (!Instance.parties.TryGetValue(partyId, out party))
            {
                return null;
            }
            return party;
        }

        public static bool DisbandParty(int partyId)
        {
            Party party;
            bool result = Instance.parties.TryRemove(partyId, out party);
            // TODO: Unsubscribe
            return result;
        }

        private static int GetNewId()
        {
            int newId = Instance.rollingPartyId;
            Interlocked.Increment(ref Instance.rollingPartyId);
            return newId;
        }

        #region Party nested class

        sealed class Party : IParty
        {
            public PartyMember Leader { get; set; }
            private List<PartyMember> members;
            public int Id { get; private set; }

            private Party()
            {
                this.members = new List<PartyMember>(6);
            }

            public Party(int id, PartyMember leader)
                : this()
            {
                this.Id = id;
                this.Leader = leader;
                this.members.Add(leader);
            }

            public void AddMember(PartyMember member)
            {
                if (member == null) throw new ArgumentNullException("member");
                if (members.Count == 6)
                {
                    throw new InvalidOperationException("This party is full.");
                }
                members.Add(member);
            }

            public void RemoveMember(PartyMember member)
            {
                if (member == null) throw new ArgumentNullException("member");
                if (member == this.Leader)
                {
                    throw new InvalidOperationException("You can't remove the leader. Disband the party instead.");
                }
                if (members.Count == 1)
                {
                    throw new InvalidOperationException("There are no members to remove.");
                }
                members.Remove(member);
            }

            public void UpdateMember(PartyMember member)
            {
                // TODO: Observer later.
            }

            public PartyMember GetMemberById(int characterId)
            {
                return this.members.FirstOrDefault(member => member.CharacterId == characterId);
            }

            #region Implementation of IEquatable<IParty>

            public bool Equals(IParty other)
            {
                return this.Id == other.Id;
            }

            #endregion
        }

        #endregion
    }

    interface IParty : IEquatable<IParty>
    {
        PartyMember Leader { get; set; }
        int Id { get; }

        void AddMember(PartyMember member);
        void RemoveMember(PartyMember member);
        void UpdateMember(PartyMember member);

        PartyMember GetMemberById(int characterId);
    }
}
