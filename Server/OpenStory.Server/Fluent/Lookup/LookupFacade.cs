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

        public ICharactersLookupFacade Characters()
        {
            return new CharactersLookupFacade(this.manager);
        }

        #endregion
    }
}