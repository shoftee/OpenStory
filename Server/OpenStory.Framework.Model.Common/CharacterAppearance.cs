using OpenStory.Common.Game;

namespace OpenStory.Framework.Model.Common
{
    public class CharacterAppearance
    {
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
    }
}