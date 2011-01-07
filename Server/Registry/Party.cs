using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenMaple.Server.Registry
{
    sealed class Party : IParty
    {
        public PartyMember Leader { get; set; }
        private List<PartyMember> members;
        public int Id { get; private set; }

        public Party(int id, PartyMember leader)
        {
            this.members = new List<PartyMember>(6);
            this.Id = id;
            this.Leader = leader;
            this.members.Add(leader);
        }

        public void AddMember(PartyMember member)
        {
            if (member == null) throw new ArgumentNullException("member");
            if (members.Count == 6) throw new InvalidOperationException("This party is full.");
            members.Add(member);
        }

        public void RemoveMember(PartyMember member)
        {
            if (member == null) throw new ArgumentNullException("member");
            if (member == this.Leader) throw new InvalidOperationException("You can't remove the leader. Disband the party instead.");
            if (members.Count == 1) throw new InvalidOperationException("There are no members to remove.");

            members.Remove(member);
        }

        public void UpdateMember(PartyMember member)
        {
            // TODO: Observer later.
        }

        public PartyMember GetMemberById(int characterId)
        {
            return this.members.FirstOrDefault(member => member.PlayerId == characterId);
        }

        #region Implementation of IEquatable<IParty>

        public bool Equals(IParty other)
        {
            return this.Id == other.Id;
        }

        #endregion
    }
}
