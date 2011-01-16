namespace OpenStory.Server.Registry
{
    /// <summary>
    /// Represents a guild emblem and its properties.
    /// </summary>
    public class GuildEmblem
    {
        /// <summary>
        /// Initializes a new instance of GuildEmblem.
        /// </summary>
        /// <param name="foregroundId">The foreground ID to assign.</param>
        /// <param name="foregroundColor">The foreground color to assign.</param>
        /// <param name="backgroundId">The background ID to assign.</param>
        /// <param name="backgroundColor">The background color to assign.</param>
        public GuildEmblem(int foregroundId, byte foregroundColor, int backgroundId, byte backgroundColor)
        {
            this.ForegroundId = foregroundId;
            this.ForegroundColor = foregroundColor;
            this.BackgroundId = backgroundId;
            this.BackgroundColor = backgroundColor;
        }

        /// <summary>
        /// Gets or sets the ID of the foreground image for the emblem.
        /// </summary>
        public int ForegroundId { get; set; }

        /// <summary>
        /// Gets or sets the color variation for the foreground image.
        /// </summary>
        public byte ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the ID of the backgruond image for the emblem.
        /// </summary>
        public int BackgroundId { get; set; }

        /// <summary>
        /// Gets or sets the color variation for the background image.
        /// </summary>
        public byte BackgroundColor { get; set; }
    }
}