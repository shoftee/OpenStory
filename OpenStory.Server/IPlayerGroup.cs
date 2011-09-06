using System;
using System.Collections.Generic;

namespace OpenStory.Server
{
    /// <summary>
    /// Provides properties and methods for registration and notification of player groups.
    /// </summary>
    /// <typeparam name="TUpdateInfo">The type that will be used for notification information.</typeparam>
    interface IPlayerGroup<in TUpdateInfo>
    {
        /// <summary>
        /// Gets the 32-bit identifier for the group.
        /// </summary>
        /// <remarks>
        /// This identifier need not be unique across all group types.
        /// </remarks>
        int Id { get; }

        /// <summary>
        /// Processes an update to the group state.
        /// </summary>
        /// <param name="updateInfo">The object containing the update information.</param>
        void Update(TUpdateInfo updateInfo);
    }

    /// <summary>
    /// Represents an abstract player group.
    /// </summary>
    /// <typeparam name="TGroupMember">The type that will be used for this group's members.</typeparam>
    /// <typeparam name="TUpdateInfo">The type that will be used for notification information.</typeparam>
    abstract class PlayerGroup<TGroupMember, TUpdateInfo> : IPlayerGroup<TUpdateInfo>
        where TGroupMember : IEquatable<TGroupMember>
    {
        /// <summary>
        /// Gets a HashSet of the members of this player group.
        /// </summary>
        protected HashSet<TGroupMember> Members { get; private set; }

        public int Id { get; private set; }

        /// <summary>
        /// Initializes a new instance of PlayerGroup with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier for the new group.</param>
        protected PlayerGroup(int id)
        {
            this.Id = id;
            this.Members = new HashSet<TGroupMember>();
        }

        /// <summary>
        /// Initializes a new instance of PlayerGroup with the specified identifier and members.
        /// </summary>
        /// <param name="id">The identifier for the new group.</param>
        /// <param name="members">The initial member list of this group.</param>
        protected PlayerGroup(int id, IEnumerable<TGroupMember> members)
        {
            this.Id = id;
            this.Members = new HashSet<TGroupMember>(members);
        }

        /// <summary>
        /// Adds a new member to the PlayerGroup.
        /// </summary>
        /// <param name="member">The new member to add.</param>
        /// <returns><c>true</c> if the member was added successfully; otherwise, <c>false</c>.</returns>
        public bool AddMember(TGroupMember member)
        {
            return this.Members.Add(member);
        }

        /// <summary>
        /// Removes a member from the PlayerGroup.
        /// </summary>
        /// <param name="member">The member to remove.</param>
        /// <returns></returns>
        public bool RemoveMember(TGroupMember member)
        {
            return this.Members.Remove(member);
        }

        public abstract void Update(TUpdateInfo updateInfo);
    }
}
