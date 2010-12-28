using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenMaple.Game;

namespace OpenMaple.Server.Registry
{
    sealed partial class PartyRegistry
    {
        private static readonly PartyRegistry Instance = new PartyRegistry();
        private PartyRegistry()
        {
            this.parties = new ConcurrentDictionary<int, Party>();
        }

        private int rollingPartyId;
        private ConcurrentDictionary<int, Party> parties;

        public static IParty CreateParty(Player leader)
        {
            if (leader == null) throw new ArgumentNullException("leader");
            if (leader.PartyMember != null) throw new InvalidOperationException("The player is already in a party.");

            PartyMember leaderMember = new PartyMember(leader);
            int newPartyId = GetNewId();
            Party party = new Party(newPartyId, leaderMember);
            if (!Instance.parties.TryAdd(newPartyId, party))
            {
                throw new Exception("what.");
            }
            return party;
        }

        public static IParty GetPartyById(int partyId)
        {
            // TODO: Persisting the party across sessions.
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
