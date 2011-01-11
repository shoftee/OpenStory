using System;

namespace OpenMaple.Server.Registry
{
    /// <summary>
    /// Provides methods for managing a party.
    /// </summary>
    public interface IParty : IEquatable<IParty>
    {
        PartyMember Leader { get; set; }
        int Id { get; }

        void AddMember(PartyMember member);
        void RemoveMember(PartyMember member);

        void ChangeLeader(PartyMember newLeader);

        PartyMember GetMemberById(int characterId);

        void Disband();
    }
}