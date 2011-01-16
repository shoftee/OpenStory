using System;

namespace OpenStory.Server.Registry
{
    /// <summary>
    /// Provides methods for managing a messenger session.
    /// </summary>
    public interface IMessenger : IEquatable<IMessenger>
    {
        /// <summary>
        /// The ID of the messenger session.
        /// </summary>
        int Id { get; }
        /// <summary>
        /// The number of players in the messenger.
        /// </summary>
        int MemberCount { get; }

        /// <summary>
        /// Adds a new member to the messenger session.
        /// </summary>
        /// <param name="member">The member to add.</param>
        void AddMember(MessengerMember member);
        /// <summary>
        /// Removes a member from the messenger session.
        /// </summary>
        /// <param name="member">The member to remove.</param>
        void RemoveMember(MessengerMember member);
    }
}