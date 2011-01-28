using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using OpenStory.Common;

namespace OpenStory.Server.Data
{
    class CharacterData
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int WorldId { get; private set; }

        public Gender Gender { get; private set; }
        public int HairId { get; private set; }
        public int FaceId { get; private set; }
        public int SkinColor { get; private set; }

        public int JobId { get; private set; }
        public int Fame { get; private set; }
        public int Level { get; private set; }

        public int BuddyListCapacity { get; private set; }

        public CharacterData(int characterId)
        {
            CharacterEngine.SelectCharacter(characterId, this.PopulateFromRecord);
        }

        private void PopulateFromRecord(IDataRecord record)
        {
            this.Id = (int) record["CharacterId"];
            this.WorldId = (int) record["WorldId"];
            this.Name = (string) record["CharacterName"];

            this.Gender = (Gender) record["Gender"];
            this.HairId = (int) record["HairId"];
            this.FaceId = (int) record["FaceId"];
            this.SkinColor = (int) record["SkinColor"];

            this.Fame = (int) record["Fame"];
            this.JobId = (int) record["JobId"];
            this.Level = (int) record["Level"];

            this.BuddyListCapacity = (int) record["BuddyListCapacity"];
        }

    }
}
