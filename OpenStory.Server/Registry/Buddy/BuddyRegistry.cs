namespace OpenStory.Server.Registry.Buddy
{
    sealed class BuddyRegistry
    {
        private static readonly BuddyRegistry Instance;
        static BuddyRegistry()
        {
            Instance = new BuddyRegistry();
        }

        private BuddyRegistry()
        {
            
        }
    }
}
