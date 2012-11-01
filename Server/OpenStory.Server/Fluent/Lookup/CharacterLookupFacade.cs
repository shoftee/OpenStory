using OpenStory.Server.Modules;

namespace OpenStory.Server.Fluent.Lookup
{
    internal sealed class CharacterLookupFacade : ICharacterLookupFacade
    {
        private readonly LookupManager manager;

        public CharacterLookupFacade(LookupManager manager)
        {
            this.manager = manager;
        }

        #region Implementation of ICharacterLookupFacade

        public PlayerLocation Location(int id)
        {
            return this.manager.Location.GetLocation(id);
        }

        public string Name(int id)
        {
            var player = this.manager.Players.GetById(id);
            if (player == null)
            {
                return null;
            }
            else
            {
                return player.CharacterName;
            }
        }

        public int? Id(string name)
        {
            var player = this.manager.Players.GetByName(name);
            if (player == null)
            {
                return null;
            }
            else
            {
                return player.CharacterId;
            }
        }

        #endregion
    }
}