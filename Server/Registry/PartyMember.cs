using System;
using OpenMaple.Game;

namespace OpenMaple.Server.Registry
{
    class PartyMember : IPlayerExtension<IPlayer>, IEquatable<PartyMember>
    {
        public int PlayerId { get; private set; }
        public string Name { get; private set; }

        public int Level { get; private set; }
        public int ChannelId { get; private set; }
        public int JobId { get; private set; }
        public int MapId { get; private set; }
        public bool IsOnline { get; private set; }

        public PartyMember(IPlayer player)
        {
            this.PlayerId = player.CharacterId;
            this.Name = player.CharacterName;
            this.Level = player.Level;
            this.ChannelId = player.ChannelId;
            this.JobId = player.JobId;
            this.MapId = player.MapId;
            this.IsOnline = true;
        }

        public void Update(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public void Release()
        {
            throw new NotImplementedException();
        }

        #region Implementation of IEquatable<PartyMember>

        public bool Equals(PartyMember other)
        {
            return this.PlayerId == other.PlayerId;
        }

        #endregion
    }
}