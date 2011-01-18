using System;
using System.Collections.Generic;
using OpenStory.Common.Threading;
using OpenStory.Server.Synchronization;

namespace OpenStory.Server.Registry
{
    internal sealed class PartyRegistry : IPartyRegistry
    {
        private static readonly PartyRegistry Instance;
        private static readonly ISynchronized<PartyRegistry> SynchronizedInstance;

        private Dictionary<int, PartyMember> members;
        private Dictionary<int, Party> parties;
        private AtomicInteger rollingPartyId = new AtomicInteger(0);

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

        /// <summary>
        /// Synchronized entry point for PartyRegistry.
        /// </summary>
        public static ISynchronized<IPartyRegistry> Synchronized
        {
            get { return SynchronizedInstance; }
        }

        #region IPartyRegistry Members

        public IParty CreateParty(IPlayer leader)
        {
            if (leader == null) throw new ArgumentNullException("leader");

            PartyMember leaderMember = this.GetMember(leader);
            if (leaderMember != null)
            {
                throw new InvalidOperationException("The player is already in a party.");
            }

            return this.CreatePartyInternal(leader);
        }

        public IParty GetPartyById(int partyId)
        {
            // TODO: Persisting the party across sessions.
            // The party needs to stay the same even if all members are offline.

            Party party;
            return this.parties.TryGetValue(partyId, out party) ? party : null;
        }

        #endregion

        private Party CreatePartyInternal(IPlayer leader)
        {
            PartyMember leaderMember = this.AddMember(leader);

            int partyId = this.rollingPartyId.Increment();

            var party = new Party(partyId, leaderMember, () => this.DisbandParty(partyId));
            this.parties.Add(partyId, party);
            return party;
        }

        private PartyMember GetMember(IPlayer player)
        {
            PartyMember member;
            return this.members.TryGetValue(player.CharacterId, out member) ? member : null;
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

        private PartyMember AddMember(IPlayer player)
        {
            var member = new PartyMember(player);
            this.members.Add(player.CharacterId, member);
            return member;
        }

        public void DisbandParty(int partyId)
        {
            this.parties.Remove(partyId);
            // TODO: Unsubscribe
        }
    }
}