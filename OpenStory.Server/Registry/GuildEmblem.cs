namespace OpenStory.Server.Registry
{
    public class GuildEmblem
    {
        public GuildEmblem(int foregroundId, byte foregroundColor, int backgroundId, byte backgroundColor)
        {
            this.ForegroundId = foregroundId;
            this.ForegroundColor = foregroundColor;
            this.BackgroundId = backgroundId;
            this.BackgroundColor = backgroundColor;
        }

        public int ForegroundId { get; set; }
        public byte ForegroundColor { get; set; }
        public int BackgroundId { get; set; }
        public byte BackgroundColor { get; set; }
    }
}