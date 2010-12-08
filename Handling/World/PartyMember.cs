using System;
using OpenMaple.Client;

namespace OpenMaple.Handling.World
{
    class PartyMember : IEquatable<PartyMember>
    {
        public int CharacterId { get; private set; }
        public string Name { get; private set; }
        public int Level { get; set; }
        public int Channel { get; set; }
        public int JobId { get; set; }
        public int MapId { get; set; }
        public bool IsOnline { get; set; }

        public PartyMember(Character character)
        {
            this.CharacterId = character.Id;
            this.Name = character.Name;
            this.Level = character.Level;
            this.Channel = character.Client.Channel;
            this.JobId = character.JobId;
            this.MapId = character.MapId;
            this.IsOnline = true;
        }

        #region Implementation of IEquatable<PartyMember>

        public bool Equals(PartyMember other)
        {
            return this.CharacterId == other.CharacterId;
        }

        #endregion
    }
}