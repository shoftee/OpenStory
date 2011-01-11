using System;

namespace OpenMaple.Server.Registry
{
    /// <summary>
    /// Provides methods for managing a messenger session.
    /// </summary>
    public interface IMessenger : IEquatable<IMessenger>
    {
        int Id { get; }
        int MemberCount { get; }

        void AddMember(MessengerMember member);
        void RemoveMember(MessengerMember member);
    }
}