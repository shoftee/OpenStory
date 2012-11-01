using OpenStory.Server.Modules;

namespace OpenStory.Server.Fluent.Lookup
{
    internal sealed class CharactersLookupFacade : ICharactersLookupFacade
    {
        private readonly LookupManager manager;

        public CharactersLookupFacade(LookupManager manager)
        {
            this.manager = manager;
        }
    }
}