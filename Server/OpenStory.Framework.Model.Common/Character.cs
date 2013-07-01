using OpenStory.Common.Game;

namespace OpenStory.Framework.Model.Common
{
    /// <summary>
    /// Represents a base class for Character objects.
    /// </summary>
    public abstract class Character
    {
        /// <summary>
        /// Gets the unique identifier for this character.
        /// </summary>
        public CharacterKey Key { get; private set; }

        /// <summary>
        /// Gets the ID of the world the Character resides in.
        /// </summary>
        public int WorldId { get; private set; }

        /// <summary>
        /// Gets the Gender of the Character.
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Gets the ID of the Character's hair.
        /// </summary>
        public int HairId { get; set; }

        /// <summary>
        /// Gets the ID of the Character's face.
        /// </summary>
        public int FaceId { get; set; }

        /// <summary>
        /// Gets the ID of the Character's skin color.
        /// </summary>
        public int SkinColorId { get; set; }

        /// <summary>
        /// Gets the ID of the Character's in-game job.
        /// </summary>
        public int JobId { get; set; }

        /// <summary>
        /// Gets the fame points of the Character.
        /// </summary>
        public int Fame { get; set; }

        /// <summary>
        /// Gets the level of the Character.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets the buddy list capacity of the Character.
        /// </summary>
        public int BuddyListCapacity { get; set; }
    }
}
