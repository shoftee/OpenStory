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
            return new CharacterLookupFacade();
        }

        #endregion
    }

    internal class CharacterLookupFacade : ICharacterLookupFacade
    {
        #region Implementation of ICharacterLookupFacade

        public PlayerLocation Location(int id)
        {
            throw new System.NotImplementedException();
        }

        public PlayerLocation Location(string name)
        {
            throw new System.NotImplementedException();
        }

        public string Name(int id)
        {
            throw new System.NotImplementedException();
        }

        public int? Id(string name)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}