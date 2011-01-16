using System.Data;
using OpenStory.Common;
using OpenStory.Server.Data;

namespace OpenStory.Server.Game
{
    /// <summary>
    /// Represents a game character.
    /// </summary>
    public class Character
    {
        private Character() { }
        /// <summary>
        /// Gets the internal ID of the Character.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the Character's in-game name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the ID of the world in which the Character resides in.
        /// </summary>
        public int WorldId { get; private set; }

        /// <summary>
        /// Gets the gender of the Character.
        /// </summary>
        public Gender Gender { get; private set; }

        /// <summary>
        /// Gets the internal ID for the hair of the Character.
        /// </summary>
        public int HairId { get; private set; }
        /// <summary>
        /// Gets the internal ID for the face of the Character.
        /// </summary>
        public int FaceId { get; private set; }
        /// <summary>
        /// Gets the skin color variation of the Character.
        /// </summary>
        public int SkinColor { get; private set; }

        /// <summary>
        /// Gets the Job ID of the character.
        /// </summary>
        public int JobId { get; private set; }

        /// <summary>
        /// Gets the Character's fame level.
        /// </summary>
        public int Fame { get; private set; }

        /// <summary>
        /// Gets the Character's level.
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// Loads a character by its ID.
        /// </summary>
        /// <param name="characterId">The ID of the character to load.</param>
        /// <returns>A Character object with the loaded data.</returns>
        public static Character LoadFromDb(int characterId)
        {
            var character = new Character();
            CharacterEngine.SelectCharacter(characterId, character.PopulateFromRecord);
            return character;
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
        }
    }
}