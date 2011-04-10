using System.Data;
using OpenStory.Common.Game;

namespace OpenStory.Server.Data
{
    /// <summary>
    /// Represents a data object mapping for the Character table.
    /// </summary>
    public partial class Character
    {
        private Character(IDataRecord record)
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

        /// <summary>
        /// Gets the ID of the Character.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the Name of the Character.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the ID of the world the Character resides in.
        /// </summary>
        public int WorldId { get; private set; }

        /// <summary>
        /// Gets the Gender of the Character.
        /// </summary>
        public Gender Gender { get; private set; }

        /// <summary>
        /// Gets the ID of the Character's hair.
        /// </summary>
        public int HairId { get; private set; }

        /// <summary>
        /// Gets the ID of the Character's face.
        /// </summary>
        public int FaceId { get; private set; }

        /// <summary>
        /// Gets the ID of the Character's skin color.
        /// </summary>
        public int SkinColor { get; private set; }

        /// <summary>
        /// Gets the ID of the Character's in-game job.
        /// </summary>
        public int JobId { get; private set; }

        /// <summary>
        /// Gets the fame points of the Character.
        /// </summary>
        public int Fame { get; private set; }

        /// <summary>
        /// Gets the level of the Character.
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// Gets the buddy list capacity of the Character.
        /// </summary>
        public int BuddyListCapacity { get; private set; }
    }
}