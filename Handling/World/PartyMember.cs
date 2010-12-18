using System;
using OpenMaple.Client;

namespace OpenMaple.Handling.World
{
    class PartyMember : IPlayerExtension, IEquatable<PartyMember>
    {
        public int PlayerId { get; private set; }
        public string Name { get; private set; }

        public int Level { get; set; }
        public int ChannelId { get; set; }
        public int JobId { get; set; }
        public int MapId { get; set; }
        public bool IsOnline { get; set; }

        public PartyMember(Character character)
        {
            this.PlayerId = character.Id;
            this.Name = character.Name;
            this.Level = character.Level;
            this.ChannelId = character.Client.ChannelId;
            this.JobId = character.JobId;
            this.MapId = character.MapId;
            this.IsOnline = true;
        }

        public void Update(Character character)
        {
            throw new NotImplementedException();
        }

        public void Complete()
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