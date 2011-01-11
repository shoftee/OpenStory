using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenMaple.Server.Registry
{
    sealed class Party : IParty
    {
        public PartyMember Leader { get; set; }
        public int Id { get; private set; }

        private Action onDisband;
        private List<PartyMember> members;

        public Party(int id, PartyMember leader, Action onDisband)
        {
            this.Id = id;
            this.Leader = leader;
            this.onDisband = onDisband;

            this.members = new List<PartyMember>(6) { leader };
        }

        public void AddMember(PartyMember member)
        {
            if (member == null) throw new ArgumentNullException("member");
            if (members.Count == 6)
            {
                throw new InvalidOperationException("The party is full.");
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

        public PartyMember GetMemberById(int characterId)
        {
            return this.members.FirstOrDefault(member => member.CharacterId == characterId);
        }

        public void ChangeLeader(PartyMember newLeader)
        {
            if (newLeader == null) throw new ArgumentNullException("newLeader");
            if (!this.members.Contains(newLeader))
            {
                throw new InvalidOperationException("The given party member is not a member of this party.");
            }
            if (this.Leader == newLeader)
            {
                throw new InvalidOperationException("The given party member is already the leader.");
            }
            this.Leader = newLeader;
        }

        public void Disband()
        {
            this.onDisband.Invoke();

            // TODO: Disband notification.
        }

        #region Implementation of IEquatable<IParty>

        public bool Equals(IParty other)
        {
            return this.Id == other.Id;
        }

        #endregion
    }
}
