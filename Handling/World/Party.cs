using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMaple.Handling.World
{
    class PartyManager
    {
        class Party : IParty
        {
            public PartyMember Leader { get; set; }
            private List<PartyMember> members;
            public int Id { get; private set; }

            private Party()
            {
                this.members = new List<PartyMember>(6);
            }

            public Party(int id, PartyMember leader) : this()
            {
                this.Id = id;
                this.Leader = leader;
                this.members.Add(leader);
            }
        }
    }

    interface IParty : IEquatable<IParty>
    {
        PartyMember Leader { get; set; }
        int Id { get; }

        void AddMember(PartyMember member);
        void RemoveMember(PartyMember member);
        void UpdateMember(PartyMember member);

        PartyMember GetMemberById(int id);
    }

    class PartyMember
    {
        public int CharacterId { get; private set; }
        public string Name { get; private set; }
        public int Level { get; set; }
        public int Channel { get; set; }
        public int JobId { get; set; }
        public int MapId { get; set; }
        public bool IsOnline { get; set; }
    }
}
