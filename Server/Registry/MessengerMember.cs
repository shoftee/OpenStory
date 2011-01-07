using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMaple.Game;

namespace OpenMaple.Server.Registry
{
    class MessengerMember : IPlayerExtension<IPlayer>, IEquatable<MessengerMember>
    {
        public int PlayerId { get; private set; }

        public int Position { get; private set; }
        public string Name { get; private set; }

        public int ChannelId { get; set; }

        public MessengerMember(IPlayer player, int position = 0)
        {
            this.Position = position;
            this.PlayerId = player.CharacterId;
            this.Name = player.CharacterName;
            this.ChannelId = player.ChannelId;
        }

        public void Update(IPlayer character)
        {
            throw new NotImplementedException();
        }

        public void Release()
        {
            throw new NotImplementedException();
        }

        public bool Equals(MessengerMember other)
        {
            if (other == null) return false;
            return this.PlayerId == other.PlayerId;
        }
    }
}
