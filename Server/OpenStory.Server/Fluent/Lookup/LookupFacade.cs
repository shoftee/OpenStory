using OpenStory.Server.Modules;

namespace OpenStory.Server.Fluent.Lookup
{
    internal sealed class LookupFacade : ILookupFacade
    {
        private readonly LookupManager manager;

        public LookupFacade()
        {
            this.manager = LookupManager.GetManager();
        }

        #region Implementation of ILookupFacade

        public ICharacterLookupFacade Character()
        {
            return new CharacterLookupFacade(this.manager);
        }

        #endregion
    }

    internal class CharacterLookupFacade : ICharacterLookupFacade
    {
        private readonly LookupManager manager;

        public CharacterLookupFacade(LookupManager manager)
        {
            this.manager = manager;
        }

        #region Implementation of ICharacterLookupFacade

        public PlayerLocation Location(int id)
        {
            return manager.Location.GetLocation(id);
        }

        public string Name(int id)
        {
            var player = manager.Players.GetById(id);
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
            var player = manager.Players.GetByName(name);
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