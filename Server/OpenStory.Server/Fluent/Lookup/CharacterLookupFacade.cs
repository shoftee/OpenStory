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

        public PlayerLocation Location(CharacterKey key)
        {
            return this.manager.Location.GetLocation(key);
        }

        public CharacterKey Character(int id)
        {
            var player = this.manager.Players.GetById(id);
            if (player == null)
            {
                return null;
            }
            else
            {
                return player.Key;
            }
        }

        public CharacterKey Character(string name)
        {
            var player = this.manager.Players.GetByName(name);
            if (player == null)
            {
                return null;
            }
            else
            {
                return player.Key;
            }
        }

        #endregion
    }
}