using System;
using System.Linq;
using OpenStory.Common.IO;
using OpenStory.Networking;
using OpenStory.Server.Registry.Buddy;

namespace OpenStory.Server.Registry
{
    partial class Player
    {
        public void HandleBuddyOperation(PacketReader reader, ServerSession serverSession)
        {
            var operation = (BuddyOperation) reader.ReadByte();

            switch (operation)
            {
                case BuddyOperation.AddBuddy:
                    this.HandleAddBuddy(reader, serverSession);
                    return;
                case BuddyOperation.AcceptRequest:
                    return;
                case BuddyOperation.RemoveBuddy:
                    return;
            }

            // If the operation is undefined...
            serverSession.Close();
        }

        private void HandleAddBuddy(PacketReader reader, ServerSession serverSession)
        {
            string name;
            if (!reader.TryReadLengthString(out name) || name.Length > 12)
            {
                serverSession.Close();
                return;
            }

            string groupName;
            if (!reader.TryReadLengthString(out groupName) || groupName.Length > 15)
            {
                serverSession.Close();
                return;
            }

            Buddy.Buddy entry = this.buddyList.FirstOrDefault(
                buddy => buddy.CharacterName.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (entry != null)
            {
                SendBuddyListOperationResult(serverSession, BuddyOperationResult.AlreadyOnList);
                return;
            }

            if (this.buddyList.IsFull)
            {
                SendBuddyListOperationResult(serverSession, BuddyOperationResult.BuddyListFull);
                return;
            }

            // TODO: Check the target buddy list stuff.
        }

        private static void SendBuddyListOperationResult(ServerSession serverSession, BuddyOperationResult buddyOperationResult)
        {
            // TODO: Finish later.
        }
    }
}