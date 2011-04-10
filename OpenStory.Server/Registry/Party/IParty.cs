using System;

namespace OpenStory.Server.Registry.Party
{
    /// <summary>
    /// Provides methods for managing a party.
    /// </summary>
    public interface IParty : IEquatable<IParty>
    {
        /// <summary>
        /// The ID of the party.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets the leader of the party.
        /// </summary>
        PartyMember Leader { get; }

        /// <summary>
        /// Adds a new party member.
        /// </summary>
        /// <param name="member">The party member to add.</param>
        void AddMember(PartyMember member);

        /// <summary>
        /// Removes a party member.
        /// </summary>
        /// <param name="member">The party member to remove.</param>
        void RemoveMember(PartyMember member);

        /// <summary>
        /// Changes the leader of the party.
        /// </summary>
        /// <param name="newLeader">The party member to assign the leader position to.</param>
        void ChangeLeader(PartyMember newLeader);

        /// <summary>
        /// Gets a party member by their character ID.
        /// </summary>
        /// <param name="characterId">The character ID of the party member.</param>
        /// <returns>A <see cref="PartyMember"/> object with the given character ID if one was found.</returns>
        PartyMember GetMemberById(int characterId);

        /// <summary>
        /// Disbands the party.
        /// </summary>
        void Disband();
    }
}