using System;
using System.Collections.Concurrent;
using OpenMaple.Tools;

namespace OpenMaple.Server.Registry
{
    partial class MessengerRegistry
    {
        private static readonly MessengerRegistry Instance = new MessengerRegistry();
        private MessengerRegistry()
        {
            this.messengers = new ConcurrentDictionary<int, Messenger>();
            this.rollingMessengerId = new AtomicInteger(0);
        }

        private readonly ConcurrentDictionary<int, Messenger> messengers;
        private AtomicInteger rollingMessengerId;

        public static IMessenger CreateMessenger(Player initiator)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (initiator.MessengerId != -1) throw new InvalidOperationException("This player already has a messenger started.");

            int messengerId = Instance.rollingMessengerId.Increment();
            MessengerMember initiatorMember = new MessengerMember(initiator);
            Messenger messenger = new Messenger(messengerId, initiatorMember, () => RemoveById(messengerId));
            if (!Instance.messengers.TryAdd(messengerId, messenger))
            {
                throw new Exception("what.");
            }
            return messenger;
        }

        public static IMessenger GetById(int messengerId)
        {
            Messenger messenger;
            if (!Instance.messengers.TryGetValue(messengerId, out messenger))
            {
                return null;
            }
            return messenger;
        }

        private static void RemoveById(int messengerId)
        {
            Messenger messenger;
            Instance.messengers.TryRemove(messengerId, out messenger);
        }
    }

    interface IMessenger
    {
        int Id { get; }
        int MemberCount { get; }

        void AddMember(MessengerMember member);
        void RemoveMember(MessengerMember member);
    }
}