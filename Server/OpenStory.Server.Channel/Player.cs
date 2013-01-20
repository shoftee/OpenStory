using OpenStory.Common.Game;

namespace OpenStory.Server.Channel
{
    internal sealed partial class Player : IPlayer
    {
        #region Visible info

        public int CharacterId { get; private set; }
        public string CharacterName { get; private set; }
        public int JobId { get; private set; }
        public int Level { get; private set; }

        public int ChannelId { get; private set; }
        public int MapId { get; private set; }

        public int WorldId { get; private set; }

        public Gender Gender { get; private set; }
        public int HairId { get; private set; }
        public int FaceId { get; private set; }
        public int SkinColor { get; private set; }

        public int Fame { get; private set; }

        #endregion

        public int Meso { get; private set; }
        public int Experience { get; private set; }

        private readonly ChannelClient client;

        ClientBase IPlayer.Client { get { return this.client; } }

        private Player(ChannelClient client, ChannelCharacter character)
        {
            this.client = client;

            // Get what we can from the transfer object.
            this.CharacterId = character.Id;
            this.CharacterName = character.Name;
            this.WorldId = character.WorldId;

            this.Gender = character.Gender;
            this.HairId = character.HairId;
            this.FaceId = character.FaceId;
            this.SkinColor = character.SkinColor;

            this.JobId = character.JobId;
            this.Fame = character.Fame;
            this.Level = character.Level;

            // TODO: There are still more things to add to ChannelCharacter
        }

        /// <summary>
        /// Gets an facet object of this player instance.
        /// </summary>
        /// <typeparam name="TPlayerFacet">The type of the facet interface.</typeparam>
        /// <returns>an instance of <typeparamref name="TPlayerFacet"/>.</returns>
        private TPlayerFacet GetFacet<TPlayerFacet>()
            where TPlayerFacet : IPlayerFacet
        {
            return PlayerFacetManager.Instance.Get<TPlayerFacet>(this.CharacterId);
        }

        /// <summary>
        /// Creates a new facet object for this player instance.
        /// </summary>
        /// <typeparam name="TPlayerFacet">The type of the facet interface.</typeparam>
        /// <returns>an instance of <typeparamref name="TPlayerFacet"/>.</returns>
        private TPlayerFacet CreateFacet<TPlayerFacet>()
            where TPlayerFacet : IPlayerFacet
        {
            return PlayerFacetManager.Instance.Create<TPlayerFacet>(this.CharacterId);
        }
    }
}
