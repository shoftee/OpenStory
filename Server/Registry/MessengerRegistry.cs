using System;
using System.Collections.Generic;
using OpenMaple.Synchronization;
using OpenMaple.Threading;

namespace OpenMaple.Server.Registry
{
    sealed class MessengerRegistry : IMessengerRegistry
    {
        /// <summary>
        /// Synchronized entry point for MessengerRegistry.
        /// </summary>
        public static ISynchronized<IMessengerRegistry> Synchronized { get { return SynchronizedInstance; } }

        private static readonly MessengerRegistry Instance;
        private static readonly ISynchronized<MessengerRegistry> SynchronizedInstance;
        static MessengerRegistry()
        {
            Instance = new MessengerRegistry();
            SynchronizedInstance = Synchronizer.Synchronize(Instance);
        }

        private MessengerRegistry()
        {
            this.messengers = new Dictionary<int, Messenger>();
            this.members = new Dictionary<int, MessengerMember>();
            this.rollingMessengerId = new AtomicInteger(0);
        }

        private readonly Dictionary<int, Messenger> messengers;
        private readonly Dictionary<int, MessengerMember> members;
        private AtomicInteger rollingMessengerId;

        public IMessenger CreateMessenger(IPlayer initiator)
        {
            if (initiator == null)
            {
                throw new ArgumentNullException("initiator");
            }
            return this.CreateMessengerInternal(initiator);
        }

        private Messenger CreateMessengerInternal(IPlayer initiator)
        {
            MessengerMember member = this.GetMember(initiator);
            if (member != null)
            {
                throw new InvalidOperationException("This player already has a messenger started.");
            }

            MessengerMember initiatorMember = this.AddMember(initiator);
            int messengerId = this.rollingMessengerId.Increment();
            return this.AddMessenger(messengerId, initiatorMember);
        }

        private MessengerMember GetMember(IPlayer player)
        {
            MessengerMember member;
            return this.members.TryGetValue(player.CharacterId, out member) ? member : null;
        }

        private MessengerMember AddMember(IPlayer player, int position = 0)
        {
            var member = new MessengerMember(player, position);
            this.members.Add(player.CharacterId, member);
            return member;
        }

        private Messenger AddMessenger(int messengerId, MessengerMember initiatorMember)
        {
            var messenger = new Messenger(messengerId, initiatorMember, () => RemoveById(messengerId));
            this.messengers.Add(messengerId, messenger);
            return messenger;
        }

        public IMessenger GetById(int messengerId)
        {
            Messenger messenger;
            return Instance.messengers.TryGetValue(messengerId, out messenger) ? messenger : null;
        }

        private void RemoveById(int messengerId)
        {
            this.messengers.Remove(messengerId);
        }
    }
}