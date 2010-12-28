using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMaple.Server.Registry
{
    partial class MessengerRegistry
    {
        class Messenger : IMessenger, IEquatable<Messenger>
        {
            private Action deletionCallback;
            public int Id { get; private set; }
            private MessengerMember[] members;
            private bool[] freePositions;

            private int memberCount;
            public int MemberCount
            {
                get { return this.memberCount; }
                private set
                {
                    this.memberCount = value;
                    if (value == 0)
                    {
                        this.deletionCallback();
                    }
                }
            }

            private Messenger()
            {
                this.members = new MessengerMember[] { null, null, null };
                this.freePositions = new[] { false, false, false };
            }

            public Messenger(int id, MessengerMember initiator, Action deleteAction)
                : this()
            {
                if (initiator == null) throw new ArgumentNullException("initiator");
                if (deleteAction == null) throw new ArgumentNullException("deleteAction");
                this.Id = id;
                this.SetPosition(0, initiator);
                this.deletionCallback = deleteAction;
            }

            public void AddMember(MessengerMember member)
            {
                if (member == null) throw new ArgumentNullException("member");
                int position = GetFreePosition();
                if (position == -1) throw new InvalidOperationException("This messenger is full.");
                this.SetPosition(position, member);
                this.MemberCount++;
            }

            public void RemoveMember(MessengerMember member)
            {
                int position = member.Position;
                this.SetPosition(position, null);
                this.MemberCount--;
            }

            private int GetFreePosition()
            {
                for (int i = 0; i < 3; i++)
                {
                    if (this.freePositions[i]) return i;
                }
                return -1;
            }

            private void SetPosition(int i, MessengerMember member)
            {
                members[i] = member;
                this.freePositions[i] = member == null;
            }

            public bool Equals(Messenger other)
            {
                if (other == null) return false;
                return this.Id == other.Id;
            }
        }
    }
}
