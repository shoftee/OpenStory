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

        ClientBase IPlayer.Client => Client;

        private Player(ChannelClient client, ChannelCharacter character)
        {
            Client = client;

            // Get what we can from the transfer object.
            Key = character.Key;
            WorldId = character.WorldId;

            Appearance = character.Appearance;

            JobId = character.JobId;
            Fame = character.Fame;
            Level = character.Level;

            // TODO: There are still more things to add to ChannelCharacter
        }
    }
}
