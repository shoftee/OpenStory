using System;
using System.Collections.Generic;
using System.Linq;
using OpenMaple.Threading;

namespace OpenMaple.Server.Registry
{
    sealed class PartyRegistry
    {
        /// <summary>
        /// Synchronized entry point for PartyRegistry.
        /// </summary>
        public static ISynchronized<PartyRegistry> Synchronized { get { return SynchronizedInstance; } }

        private static readonly PartyRegistry Instance;
        private static readonly ISynchronized<PartyRegistry> SynchronizedInstance;
        static PartyRegistry()
        {
            Instance = new PartyRegistry();
            SynchronizedInstance = Synchronizer.Synchronize(Instance);
        }

        private PartyRegistry()
        {
            this.parties = new Dictionary<int, Party>();
            this.members = new Dictionary<int, PartyMember>();
            // TODO: Load the initial value from DB.
            this.rollingPartyId = new AtomicInteger(0);
        }

        private AtomicInteger rollingPartyId = new AtomicInteger(0);

        private Dictionary<int, Party> parties;
        private Dictionary<int, PartyMember> members;

        public IParty CreateParty(IPlayer leader)
        {
            if (leader == null) throw new ArgumentNullException("leader");
            return this.CreatePartyInternal(leader);
        }

        private IParty CreatePartyInternal(IPlayer leader)
        {
            PartyMember leaderMember = this.GetMember(leader);
            if (leaderMember != null)
            {
                throw new InvalidOperationException("The player is already in a party.");
            }

            leaderMember = this.GetOrAddMember(leader);
            int partyId = this.rollingPartyId.Increment();
            Party party = new Party(partyId, leaderMember);
            this.parties.Add(partyId, party);
            return party;
        }

        private PartyMember GetMember(IPlayer player)
        {
            PartyMember member;
            if (!this.members.TryGetValue(player.CharacterId, out member))
            {
                return null;
            }
            return member;
        }

        private PartyMember GetOrAddMember(IPlayer player)
        {
            PartyMember member;
            int id = player.CharacterId;
            if (!this.members.TryGetValue(id, out member))
            {
                member = new PartyMember(player);
                this.members.Add(id, member);
            }
            return member;
        }

        public IParty GetPartyById(int partyId)
        {
            // TODO: Persisting the party across sessions.
            // The party needs to stay the same even if all members are offline.

            Party party;
            if (!this.parties.TryGetValue(partyId, out party))
            {
                return null;
            }
            return party;
        }

        public bool DisbandParty(int partyId)
        {
            bool result = this.parties.Remove(partyId);
            // TODO: Unsubscribe
            return result;
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
