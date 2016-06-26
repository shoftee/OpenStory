using OpenStory.Framework.Model.Common;
using OpenStory.Server.Processing;

namespace OpenStory.Server.Channel
{
    internal sealed partial class Player : IPlayer
    {
        #region Visible info

        public CharacterKey Key { get; }

        public CharacterAppearance Appearance { get; }

        public int JobId { get; private set; }

        public int Level { get; private set; }

        public int WorldId { get; private set; }

        public int ChannelId { get; private set; }

        public int MapId { get; private set; }

        public int Fame { get; private set; }

        #endregion

        public int Meso { get; private set; }

        public int Experience { get; private set; }

        public ChannelClient Client { get; set; }

        ClientBase IPlayer.Client
        {
            get { return this.Client; }
        }

        private Player(ChannelClient client, ChannelCharacter character)
        {
            this.Client = client;

            // Get what we can from the transfer object.
            this.Key = character.Key;
            this.WorldId = character.WorldId;

            this.Appearance = character.Appearance;

            this.JobId = character.JobId;
            this.Fame = character.Fame;
            this.Level = character.Level;

            // TODO: There are still more things to add to ChannelCharacter
        }
    }
}
